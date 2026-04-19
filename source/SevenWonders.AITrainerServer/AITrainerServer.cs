using GameLogic;
using SevenWonders.AI.Model.DecisionRouter.DecisionHandlers;
using SevenWonders.AI.Model.Messages;
using SevenWonders.AI.Model.Services;
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
                               IPlayerActionMaskReceiver playerActionMaskReceiver,
                               IRandomGeneratorFactory randomGeneratorFactory,
                               IAIDecisionHandler aIDecisionHandler,
                               INonPlayerActionReceiverFactory nonPlayerActionReceiverFactory)
        {
            m_game = game;
            m_playerActionMaskReceiver = playerActionMaskReceiver;
            m_randomGeneratorFactory = randomGeneratorFactory;
            m_aIDecisionHandler = aIDecisionHandler;
            m_nonPlayerActionReceiverFactory = nonPlayerActionReceiverFactory;
            m_gameStateResponse = null;
        }

        public void StartServer()
        {
            m_aIDecisionHandler.OnGameStateReceived += OnGameStateReceived;
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
                                        m_aIDecisionHandler.Decide(request);
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
                                        Terminated = false
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
                m_aIDecisionHandler.OnGameStateReceived -= OnGameStateReceived;
                m_server.Stop();
            }
        }

        public void Dispose()
        {
            m_gameStateReceivedEvent?.Dispose();
        }

        private void RunGame()
        {
            GameLog.Info("RunGame: Joining previous game thread...");
            m_gameThread?.Join();
            GameLog.Info("RunGame: Uninitializing AI decision handler...");
            m_aIDecisionHandler.Uninitialize();

            var aiReceiver = m_nonPlayerActionReceiverFactory.CreateNonPlayerActionReceiver(NonPlayerType.AI);
            //var heuristicReceiver = m_nonPlayerActionReceiverFactory.CreateNonPlayerActionReceiver(NonPlayerType.HeuristicBot);
            var randomReceiver = m_nonPlayerActionReceiverFactory.CreateNonPlayerActionReceiver(NonPlayerType.RandomBot);

            IRandomGenerator randomGenerator = m_randomGeneratorFactory.Create(RandomGeneratorType.Undeterministic, 0);
            m_game.Initialize(randomGenerator,
                ("AIPlayer", aiReceiver),
                ("RandomBot", randomReceiver));
            m_aIDecisionHandler.Initialize();
            m_playerActionMaskReceiver.Initialize();
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

        private void OnGameStateReceived(GameStateResponse response)
        {
            GameLog.Info($"OnGameStateReceived: Reward={response.Reward}, Terminated={response.Terminated}, StateSize={response.State?.Count}, MaskSize={response.Mask?.Count}");
            m_gameStateResponse = response;
            m_gameStateReceivedEvent.Set();
        }

        private TcpListener? m_server;
        private Thread? m_gameThread;
        private volatile GameStateResponse? m_gameStateResponse;
        private readonly object m_gameStateLock = new();
        private readonly ManualResetEventSlim m_gameStateReceivedEvent = new(false);
        private readonly IGame m_game;
        private readonly IPlayerActionMaskReceiver m_playerActionMaskReceiver;
        private readonly IRandomGeneratorFactory m_randomGeneratorFactory;
        private readonly IAIDecisionHandler m_aIDecisionHandler;
        private readonly INonPlayerActionReceiverFactory m_nonPlayerActionReceiverFactory;
    }
}