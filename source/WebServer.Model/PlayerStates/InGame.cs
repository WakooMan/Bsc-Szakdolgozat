using GameLogic.Elements;
using GameLogic.Interfaces;
using Microsoft.AspNetCore.SignalR;
using WebServer.Contract.Messages.Game.ClientMessages;
using WebServer.Contract.Messages.Game.ServerMessages;
using WebServer.Contract.Messages.Lobby.ServerMessages;
using WebServer.Model.Client;
using WebServer.Model.Lobby;
using WebServer.Model.MessageHandling;
using WebServer.Model.PlayerStates.Factories;
using WebServer.Model.ServerHub;

namespace WebServer.Model.PlayerStates
{
    public class InGame : PlayerState, IPlayerActionReceiver, IMessageHandler, IDisposable
    {
        public InGame(IPlayerStateFactory playerStateFactory, IPlayerClient player, IServerService serverService, ILobbyCodeGenerator lobbyCodeGenerator, IGameManager gameManager, IServerMessageDispatcher serverMessageDispatcher, string gameCode) : base(player, serverService, playerStateFactory, lobbyCodeGenerator)
        {
            m_gameManager = gameManager;
            m_gameCode = gameCode;
            m_serverMessageDispatcher = serverMessageDispatcher;
            m_signal = new ManualResetEventSlim(false);
            m_playerActions = new List<PlayerActionWrapper>();
            m_playerActionRequestMessageHandler = new GameRequestMessageHandlerDelegate<PlayerActionRequestMessage>(HandlePlayerActionRequestMessage);
            m_serverMessageDispatcher.RegisterHandler(this);
        }

        public override Task<LobbyServerMessage> CreateLobby(string name)
        {
            throw new NotSupportedException("Cannot execute action in this state!");
        }

        public override async Task<LobbyServerMessage> ExitGame()
        {
            m_player.ChangeState(m_playerStateFactory.CreateInMainMenuState(m_player));
            await m_serverService.LeaveGroup(m_player.ConnectionId, m_gameCode);
            await m_serverService.JoinGroup(m_player.ConnectionId, nameof(InMainMenu));
            m_lobbyCodeGenerator.RemoveUniqueCode(m_gameCode);
            m_gameManager.RemoveGame(m_gameCode);
            return new ExitGameResponseMessage(true, "OK");
        }

        public override Task<LobbyServerMessage> ExitMatchmaking()
        {
            throw new NotSupportedException("Cannot execute action in this state!");
        }

        public override Task<LobbyServerMessage> JoinLobby(string code)
        {
            throw new NotSupportedException("Cannot execute action in this state!");
        }

        public override Task<LobbyServerMessage> LeaveLobby()
        {
            throw new NotSupportedException("Cannot execute action in this state!");
        }

        public override Task<LobbyServerMessage> StartGame()
        {
            throw new NotSupportedException("Cannot execute action in this state!");
        }

        public override Task<LobbyServerMessage> StartMatchmaking()
        {
            throw new NotSupportedException("Cannot execute action in this state!");
        }

        public override Task<LobbyServerMessage> WriteChatMessage(string message)
        {
            throw new NotSupportedException("Cannot execute action in this state!");
        }

        public PlayerActionWrapper ReceivePlayerAction(Player player, ICollection<PlayerActionWrapper> playerActions)
        {
            m_chosenPlayerAction = null;
            m_signal.Reset();
            m_playerActions.Clear();
            m_playerActions.AddRange(playerActions);

            while (m_chosenPlayerAction is null)
            {
                m_signal.Wait();

                if (m_chosenPlayerAction is not null)
                {
                    return m_chosenPlayerAction;
                }
            }

            throw new InvalidOperationException($"No matching playeraction.");
        }

        private Task<GameServerMessage> HandlePlayerActionRequestMessage(Hub hub, string connectionId, PlayerActionRequestMessage message)
        {
            if (m_player.ConnectionId != connectionId)
            {
                return Task.FromResult<GameServerMessage>(new PlayerActionResponseMessage("Wrong player sent the action!"));
            }

            if (message.ActionId >= 0 && message.ActionId < m_playerActions.Count)
            {
                m_chosenPlayerAction = m_playerActions[message.ActionId];
                m_signal.Set();
                m_serverService.SendGameServerMessageToGroup(m_gameCode, new ServerPlayerActionMessage(m_player.ApplicationUser.UserName, message.ActionId), m_player.ConnectionId);
                return Task.FromResult<GameServerMessage>(new PlayerActionResponseMessage(m_player.ApplicationUser.UserName, message.ActionId));
            }
            return Task.FromResult<GameServerMessage>(new PlayerActionResponseMessage("The received action id is not a valid player action!"));
        }

        public void Dispose()
        {
            m_serverMessageDispatcher?.UnregisterHandler(this);
            m_signal?.Dispose();
        }

        public void Register(IMessageRegisterer registerer)
        {
            registerer.Register(m_playerActionRequestMessageHandler);
        }

        public void Unregister(IMessageRegisterer registerer)
        {
            registerer.Unregister(m_playerActionRequestMessageHandler);
        }

        private readonly string m_gameCode;
        private readonly IGameManager m_gameManager;
        private readonly ManualResetEventSlim m_signal;
        private readonly List<PlayerActionWrapper> m_playerActions;
        private readonly GameRequestMessageHandlerDelegate<PlayerActionRequestMessage> m_playerActionRequestMessageHandler;
        private readonly IServerMessageDispatcher m_serverMessageDispatcher;
        private PlayerActionWrapper? m_chosenPlayerAction;
    }
}
