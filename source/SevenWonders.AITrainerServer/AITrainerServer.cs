using GameLogic;
using SevenWonders.AI.Model.AIModelHandler;
using SevenWonders.AI.Model.Cache;
using SevenWonders.AI.Model.DataModel;
using SevenWonders.AI.Model.Messages;
using SevenWonders.AITrainerServer.DataModel;
using SevenWonders.AITrainerServer.PlayerActionReceivers;
using SevenWonders.Common;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;

namespace SevenWonders.AITrainerServer
{
    public class AITrainerServer: IAITrainerServer, IDisposable
    {

        public AITrainerServer(IGame game,
                               IRandomGeneratorFactory randomGeneratorFactory,
                               IAIDecisionHandlerCache aIDecisionHandlerCache,
                               IAIModelHandlerCache aIModelHandlerCache,
                               INonPlayerActionReceiverFactory nonPlayerActionReceiverFactory,
                               IEnemyChanceConfiguration enemyChanceConfiguration)
        {
            m_game = game;
            m_randomGeneratorFactory = randomGeneratorFactory;
            m_aIDecisionHandlerCache = aIDecisionHandlerCache;
            m_aIModelHandlerCache = aIModelHandlerCache;
            m_nonPlayerActionReceiverFactory = nonPlayerActionReceiverFactory;
            m_enemyChanceConfiguration = enemyChanceConfiguration;
            m_gameStateResponse = null;
        }

        public async Task StartServer()
        {
            await m_aIModelHandlerCache.EasyAIModelHandler.Initialize();
            m_aIDecisionHandlerCache.MediumAI.OnGameStateReceived = OnGameStateReceived;
            m_server = new TcpListener(IPAddress.Parse("127.0.0.1"), 5000);
            m_server.Start();
            GameLog.Info("Waiting for AI connection on port 5000...");

            using TcpClient client = m_server.AcceptTcpClient();
            using NetworkStream stream = client.GetStream();
            StreamReader reader = new(stream, new UTF8Encoding(false));
            StreamWriter writer = new(stream, new UTF8Encoding(false)) { AutoFlush = true };

            GameLog.Info("AI joined!");
            try
            {
                while (true)
                {
                    string? message = reader.ReadLine();
                    if (message is null)
                        break;

                    message = message.Trim();

                    BaseMessage? clientCommand = JsonSerializer.Deserialize<BaseMessage>(message);
                    if (clientCommand is not null)
                    {
                        lock (m_gameStateLock)
                        {
                            switch (clientCommand.MessageType)
                            {

                                case MessageType.ActionRequest:
                                    GameLog.Info("Received Action command from AI.");
                                    var request = JsonSerializer.Deserialize<ActionRequest>(clientCommand.Payload);
                                    if (request is not null)
                                    {
                                        m_actionRequest = request;
                                        m_actionRequestReceivedEvent.Set();
                                    }
                                    WaitForGameStateResponse();
                                    BaseMessage stateResponse = new()
                                    {
                                        MessageType = MessageType.GameStateResponse,
                                        Payload = JsonSerializer.Serialize(m_gameStateResponse)
                                    };
                                    string jsonStateResponse = JsonSerializer.Serialize(stateResponse);
                                    writer.WriteLine(jsonStateResponse);
                                    m_gameStateResponse = null;
                                    break;
                                case MessageType.ResetRequest:
                                    GameLog.Info("Received RESET command from AI.");
                                    RunGame();
                                    WaitForGameStateResponse();
                                    GameResetResponse gameResetResponse = new()
                                    {
                                        State = m_gameStateResponse.State,
                                        Mask = m_gameStateResponse.Mask,
                                        Terminated = false,
                                        OpponentType = (int)m_currentOpponentType
                                    };
                                    BaseMessage response = new()
                                    {
                                        MessageType = MessageType.GameResetResponse,
                                        Payload = JsonSerializer.Serialize(gameResetResponse)
                                    };
                                    string jsonResponse = JsonSerializer.Serialize(response);
                                    writer.WriteLine(jsonResponse);
                                    m_gameStateResponse = null;
                                    break;
                                case MessageType.ExitRequest:
                                    GameLog.Info("Received EXIT command from AI.");
                                    return;
                                default:
                                    GameLog.Info($"Received unknown command: {clientCommand.MessageType}");
                                    continue;
                            }
                        }
                    }
                }
            }
            finally 
            {
                GameLog.Info("Stopping server...");
                m_server.Stop();
            }
        }

        public void Dispose()
        {
            m_gameStateReceivedEvent?.Dispose();
            m_actionRequestReceivedEvent?.Dispose();
        }

        private void RunGame()
        {
            GameLog.Info("RunGame: Joining previous game thread...");
            m_gameThread?.Join();
            GameLog.Info("RunGame: Uninitializing AI decision handler...");
            m_aIDecisionHandlerCache.HardAI.Uninitialize();
            m_aIDecisionHandlerCache.MediumAI.Uninitialize();
            m_aIDecisionHandlerCache.EasyAI.Uninitialize();

            IRandomGenerator randomGenerator = m_randomGeneratorFactory.Create(RandomGeneratorType.Undeterministic, 0);
            var aiReceiver = m_nonPlayerActionReceiverFactory.CreateNonPlayerActionReceiver(NonPlayerType.MediumAI);
            EnemyChances chances = m_enemyChanceConfiguration.Chances;
            int generatedValue = randomGenerator.Next(chances.MinValue, chances.MaxValue);
            foreach (EnemyChance chance in chances.Chances.OrderBy(ch => ch.MaxValue))
            {
                if (generatedValue <= chance.MaxValue)
                {
                    m_currentOpponentType = chance.PlayerType;
                    break;
                }
            }
            GameLog.Info($"RunGame: Selected opponent type: {m_currentOpponentType}");
            var randomReceiver = m_nonPlayerActionReceiverFactory.CreateNonPlayerActionReceiver(m_currentOpponentType);
            m_game.Initialize(randomGenerator,
                ("RandomBot", randomReceiver),
                ("AIPlayer", aiReceiver));
            switch(m_currentOpponentType)
            {
                case NonPlayerType.EasyAI:
                    m_aIDecisionHandlerCache.EasyAI.Initialize(1);
                    m_aIModelHandlerCache.EasyAIModelHandler.LoadModel(AIModelType.Easy);
                    break;
                case NonPlayerType.MediumAI:
                    m_aIDecisionHandlerCache.MediumAI.Initialize(1);
                    m_aIModelHandlerCache.MediumAIModelHandler.LoadModel(AIModelType.Medium);
                    break;
            }
            m_aIDecisionHandlerCache.HardAI.Initialize(2);
            GameLog.Info("RunGame: Initialized handlers. Starting game thread...");

            m_gameThread = new Thread(() =>
            {
                GameLog.Info("Game thread started. Running GameLoop...");
                m_game.GameLoop();
                GameLog.Info("GameLoop finished.");
            });
            m_gameThread.Start();
        }

        private void WaitForGameStateResponse()
        {
            GameLog.Info("Waiting for game state response...");
            m_gameStateReceivedEvent.Wait();
            m_gameStateReceivedEvent.Reset();
            GameLog.Info("Game state response received.");
        }

        private ActionRequest OnGameStateReceived(GameStateResponse response)
        {
            GameLog.Info($"OnGameStateReceived: Reward={response.Reward}, Terminated={response.Terminated}, StateSize={response.State?.Count}, MaskSize={response.Mask?.Count}");
            m_gameStateResponse = response;
            m_gameStateReceivedEvent.Set();

            if(response.Terminated)
            {
                GameLog.Info("OnGameStateReceived: Game terminated. Exiting...");
                return null!;
            }

            GameLog.Info("OnGameStateReceived: Waiting for action request from client...");
            m_actionRequestReceivedEvent.Wait();
            m_actionRequestReceivedEvent.Reset();
            GameLog.Info("OnGameStateReceived: Action request received.");
            return m_actionRequest!;
        }

        private TcpListener? m_server;
        private Thread? m_gameThread;
        private NonPlayerType m_currentOpponentType;
        private volatile GameStateResponse? m_gameStateResponse;
        private readonly object m_gameStateLock = new();
        private readonly ManualResetEventSlim m_gameStateReceivedEvent = new(false);
        private readonly ManualResetEventSlim m_actionRequestReceivedEvent = new(false);
        private volatile ActionRequest? m_actionRequest;
        private readonly IGame m_game;
        private readonly IRandomGeneratorFactory m_randomGeneratorFactory;
        private readonly IAIDecisionHandlerCache m_aIDecisionHandlerCache;
        private readonly IAIModelHandlerCache m_aIModelHandlerCache;
        private readonly INonPlayerActionReceiverFactory m_nonPlayerActionReceiverFactory;
        private readonly IEnemyChanceConfiguration m_enemyChanceConfiguration;
    }
}