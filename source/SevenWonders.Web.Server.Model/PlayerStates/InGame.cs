using SevenWonders.Common;
using SevenWonders.Game.Logic.Elements;
using SevenWonders.Game.Logic.Exceptions;
using SevenWonders.Game.Logic.Interfaces;
using SevenWonders.Web.Server.Contract.Messages.Game.ClientMessages;
using SevenWonders.Web.Server.Contract.Messages.Game.ServerMessages;
using SevenWonders.Web.Server.Contract.Messages.Lobby.ServerMessages;
using SevenWonders.Web.Server.Model.Client;
using SevenWonders.Web.Server.Model.Lobby;
using SevenWonders.Web.Server.Model.MessageHandling;
using SevenWonders.Web.Server.Model.PlayerStates.Factories;
using SevenWonders.Web.Server.Model.ServerHub;

namespace SevenWonders.Web.Server.Model.PlayerStates
{
    public class InGame : PlayerState, IPlayerActionReceiver, IMessageHandler, IDisposable
    {
        public InGame(IPlayerStateFactory playerStateFactory,
                      IPlayerClient player,
                      IServerService serverService,
                      ILobbyCodeGenerator lobbyCodeGenerator,
                      IGameManager gameManager,
                      IServerMessageDispatcher serverMessageDispatcher,
                      ILobbyManager lobbyManager,
                      string gameCode) : base(player, serverService, playerStateFactory, lobbyCodeGenerator)
        {
            m_gameManager = gameManager;
            m_gameCode = gameCode;
            m_serverMessageDispatcher = serverMessageDispatcher;
            m_signal = new ManualResetEventSlim(false);
            m_playerActions = new List<PlayerActionWrapper>();
            m_lobbyManager = lobbyManager;
            m_playerActionRequestMessageHandler = new GameRequestMessageHandlerDelegate<PlayerActionRequestMessage>(HandlePlayerActionRequestMessage);
            m_serverMessageDispatcher.RegisterHandler(this);
            m_isEnded = false;
        }

        public override Task CreateLobby(string name)
        {
            throw new NotSupportedException("Cannot execute action in this state!");
        }

        public override async Task ExitGame()
        {
            m_player.ChangeState(m_playerStateFactory.CreateInMainMenuState(m_player));
            await m_serverService.LeaveGroup(m_player.ConnectionId, m_gameCode);
            await m_serverService.JoinGroup(m_player.ConnectionId, nameof(InMainMenu));
            m_lobbyCodeGenerator.RemoveUniqueCode(m_gameCode);
            await m_gameManager.RemoveGame(m_gameCode);
            await m_serverService.SendLobbyServerMessageToClient(m_player.ConnectionId, new ExitGameResponseMessage(m_lobbyManager.GetLobbies().Select(lobby => lobby.ToDto()).ToArray()));
        }

        public override Task ExitMatchmaking()
        {
            throw new NotSupportedException("Cannot execute action in this state!");
        }

        public override Task JoinLobby(string code)
        {
            throw new NotSupportedException("Cannot execute action in this state!");
        }

        public override Task LeaveLobby()
        {
            throw new NotSupportedException("Cannot execute action in this state!");
        }

        public override Task StartGame()
        {
            throw new NotSupportedException("Cannot execute action in this state!");
        }

        public override Task StartMatchmaking()
        {
            throw new NotSupportedException("Cannot execute action in this state!");
        }

        public override Task WriteChatMessage(string message)
        {
            throw new NotSupportedException("Cannot execute action in this state!");
        }

        public PlayerActionWrapper ReceivePlayerAction(Player player, ICollection<PlayerActionWrapper> playerActions)
        {
            m_chosenPlayerAction = null;
            m_signal.Reset();
            m_playerActions.Clear();
            m_playerActions.AddRange(playerActions);

            while (m_chosenPlayerAction is null && !m_isEnded)
            {
                m_signal.Wait();

                if (m_chosenPlayerAction is not null)
                {
                    return m_chosenPlayerAction;
                }
            }

            if (m_isEnded)
            {
                throw new EndGameException();
            }

            throw new InvalidOperationException($"No matching playeraction.");
        }

        private async Task HandlePlayerActionRequestMessage(string connectionId, PlayerActionRequestMessage message)
        {
            if (m_player.ConnectionId != connectionId)
            {
                return;
            }

            if (!m_playerActions.Select(action => action.PlayerAction.Id).SequenceEqual(message.Actions))
            {
                m_player.ChangeState(m_playerStateFactory.CreateInMainMenuState(m_player));
                await m_serverService.LeaveGroup(m_player.ConnectionId, m_gameCode);
                await m_serverService.JoinGroup(m_player.ConnectionId, nameof(InMainMenu));
                m_lobbyCodeGenerator.RemoveUniqueCode(m_gameCode);
                await m_gameManager.RemoveGame(m_gameCode);
                await m_serverService.SendLobbyServerMessageToGroup(m_gameCode, new ExitGameResponseMessage(m_lobbyManager.GetLobbies().Select(lobby => lobby.ToDto()).ToArray()));
                return;
            }

            if (message.ActionId >= 0 && message.ActionId < m_playerActions.Count)
            {
                m_chosenPlayerAction = m_playerActions[message.ActionId];
                m_signal.Set();
                await m_serverService.SendGameServerMessageToGroup(m_gameCode, new ServerPlayerActionMessage(m_player.ApplicationUser.UserName, message.ActionId), m_player.ConnectionId);
                await m_serverService.SendGameServerMessageToClient(m_player.ConnectionId, new PlayerActionResponseMessage(m_player.ApplicationUser.UserName, message.ActionId));
                return;
            }
            await m_serverService.SendGameServerMessageToClient(m_player.ConnectionId, new PlayerActionResponseMessage("The received action id is not a valid player action!"));
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

        public void EndGame()
        {
            m_isEnded = true;
            m_chosenPlayerAction = null;
            m_signal.Set();
        }

        private readonly string m_gameCode;
        private readonly IGameManager m_gameManager;
        private readonly ManualResetEventSlim m_signal;
        private readonly List<PlayerActionWrapper> m_playerActions;
        private readonly GameRequestMessageHandlerDelegate<PlayerActionRequestMessage> m_playerActionRequestMessageHandler;
        private readonly IServerMessageDispatcher m_serverMessageDispatcher;
        private readonly ILobbyManager m_lobbyManager;
        private PlayerActionWrapper? m_chosenPlayerAction;
        private bool m_isEnded;
    }
}
