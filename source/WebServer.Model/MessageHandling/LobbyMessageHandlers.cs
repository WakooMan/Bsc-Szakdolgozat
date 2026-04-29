using WebServer.Contract.DataTransferObjects;
using WebServer.Contract.Messages.Lobby.ClientMessages;
using WebServer.Contract.Messages.Lobby.ServerMessages;
using WebServer.Model.Client;
using WebServer.Model.Lobby;
using WebServer.Model.ServerHub;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;

namespace WebServer.Model.MessageHandling
{
    public class LobbyMessageHandlers : ILobbyMessageHandlers
    {
        public LobbyMessageHandlers(IClientManager clientManager, IServerService serverService, ILobbyManager lobbyManager, IServiceScopeFactory serviceScopeFactory)
        {
            m_clientManager = clientManager;
            m_serverService = serverService;
            m_lobbyManager = lobbyManager;
            m_serviceScopeFactory = serviceScopeFactory;
            m_createLobbyRequestMessageHandler = new LobbyRequestMessageHandlerDelegate<CreateLobbyRequestMessage>(OnCreateLobbyMessageReceived);
            m_joinLobbyRequestMessageHandler = new LobbyRequestMessageHandlerDelegate<JoinLobbyRequestMessage>(OnJoinLobbyMessageReceived);
            m_leaveLobbyRequestMessageHandler = new LobbyRequestMessageHandlerDelegate<LeaveLobbyRequestMessage>(OnLeaveLobbyMessageReceived);
            m_startGameRequestMessageHandler = new LobbyRequestMessageHandlerDelegate<StartGameRequestMessage>(OnStartGameMessageReceived);
            m_startMatchmakingRequestMessageHandler = new LobbyRequestMessageHandlerDelegate<StartMatchmakingRequestMessage>(OnStartMatchmakingMessageReceived);
            m_stopMatchmakingRequestMessageHandler = new LobbyRequestMessageHandlerDelegate<StopMatchmakingRequestMessage>(OnStopMatchmakingMessageReceived);
            m_sendChatRequestMessageHandler = new LobbyRequestMessageHandlerDelegate<SendChatRequestMessage>(OnSendChatMessageReceived);
            m_exitGameRequestMessageHandler = new LobbyRequestMessageHandlerDelegate<ExitGameRequestMessage>(OnExitGameMessageReceived);
            m_getLobbiesRequestMessageHandler = new LobbyRequestMessageHandlerDelegate<GetLobbiesRequestMessage>(OnGetLobbiesMessageReceived);
            m_getLeaderboardRequestMessageHandler = new LobbyRequestMessageHandlerDelegate<GetLeaderboardRequestMessage>(OnGetLeaderboardMessageReceived);
        }

        public void Register(IMessageRegisterer registerer)
        {
            registerer.Register(m_createLobbyRequestMessageHandler);
            registerer.Register(m_joinLobbyRequestMessageHandler);
            registerer.Register(m_leaveLobbyRequestMessageHandler);
            registerer.Register(m_startGameRequestMessageHandler);
            registerer.Register(m_startMatchmakingRequestMessageHandler);
            registerer.Register(m_stopMatchmakingRequestMessageHandler);
            registerer.Register(m_sendChatRequestMessageHandler);
            registerer.Register(m_exitGameRequestMessageHandler);
            registerer.Register(m_getLobbiesRequestMessageHandler);
            registerer.Register(m_getLeaderboardRequestMessageHandler);
        }

        public void Unregister(IMessageRegisterer registerer)
        {
            registerer.Unregister(m_createLobbyRequestMessageHandler);
            registerer.Unregister(m_joinLobbyRequestMessageHandler);
            registerer.Unregister(m_leaveLobbyRequestMessageHandler);
            registerer.Unregister(m_startGameRequestMessageHandler);
            registerer.Unregister(m_startMatchmakingRequestMessageHandler);
            registerer.Unregister(m_stopMatchmakingRequestMessageHandler);
            registerer.Unregister(m_sendChatRequestMessageHandler);
            registerer.Unregister(m_exitGameRequestMessageHandler);
            registerer.Unregister(m_getLobbiesRequestMessageHandler);
            registerer.Unregister(m_getLeaderboardRequestMessageHandler);
        }

        private async Task OnCreateLobbyMessageReceived(string connectionId, CreateLobbyRequestMessage requestMessage)
        {
            try
            {
                IPlayerClient playerClient = m_clientManager.GetClient(connectionId);
                await playerClient.CreateLobby(requestMessage.Name);
            } 
            catch (Exception ex)
            {
                await m_serverService.SendLobbyServerMessageToClient(connectionId, new FailureResponseMessage(false, ex.Message));
            }
        }

        private async Task OnStopMatchmakingMessageReceived(string connectionId, StopMatchmakingRequestMessage message)
        {
            try
            {
                IPlayerClient playerClient = m_clientManager.GetClient(connectionId);
                await playerClient.ExitMatchmaking();
            }
            catch (Exception ex)
            {
                await m_serverService.SendLobbyServerMessageToClient(connectionId, new FailureResponseMessage(false, ex.Message));
            }
        }

        private async Task OnStartMatchmakingMessageReceived(string connectionId, StartMatchmakingRequestMessage message)
        {
            try
            {
                IPlayerClient playerClient = m_clientManager.GetClient(connectionId);
                await playerClient.StartMatchmaking();
            }
            catch (Exception ex)
            {
                await m_serverService.SendLobbyServerMessageToClient(connectionId, new FailureResponseMessage(false, ex.Message));
            }
        }

        private async Task OnLeaveLobbyMessageReceived(string connectionId, LeaveLobbyRequestMessage message)
        {
            try
            {
                IPlayerClient playerClient = m_clientManager.GetClient(connectionId);
                await playerClient.LeaveLobby();
            }
            catch (Exception ex)
            {
                await m_serverService.SendLobbyServerMessageToClient(connectionId, new FailureResponseMessage(false, ex.Message));
            }
        }

        private async Task OnStartGameMessageReceived(string connectionId, StartGameRequestMessage message)
        {
            try
            {
                IPlayerClient playerClient = m_clientManager.GetClient(connectionId);
                await playerClient.StartGame();
            }
            catch (Exception ex)
            {
                await m_serverService.SendLobbyServerMessageToClient(connectionId, new FailureResponseMessage(false, ex.Message));
            }
        }

        private async Task OnJoinLobbyMessageReceived(string connectionId, JoinLobbyRequestMessage message)
        {
            try
            {
                IPlayerClient playerClient = m_clientManager.GetClient(connectionId);
                await playerClient.JoinLobby(message.Code);
            }
            catch (Exception ex)
            {
                await m_serverService.SendLobbyServerMessageToClient(connectionId, new FailureResponseMessage(false, ex.Message));
            }
        }

        private async Task OnSendChatMessageReceived(string connectionId, SendChatRequestMessage message)
        {
            try
            {
                IPlayerClient playerClient = m_clientManager.GetClient(connectionId);
                await playerClient.WriteChatMessage(message.Message);
            }
            catch (Exception ex)
            {
                await m_serverService.SendLobbyServerMessageToClient(connectionId, new FailureResponseMessage(false, ex.Message));
            }
        }

        private async Task OnExitGameMessageReceived(string connectionId, ExitGameRequestMessage message)
        {
            try
            {
                IPlayerClient playerClient = m_clientManager.GetClient(connectionId);
                await playerClient.ExitGame();
            }
            catch (Exception ex)
            {
                await m_serverService.SendLobbyServerMessageToClient(connectionId, new FailureResponseMessage(false, ex.Message));
            }
        }

        private async Task OnGetLobbiesMessageReceived(string connectionId, GetLobbiesRequestMessage message)
        {
            try
            {
                LobbyDto[] lobbies = m_lobbyManager.GetLobbies().Select(lobby => lobby.ToDto()).ToArray();
                await m_serverService.SendLobbyServerMessageToClient(connectionId, new LobbyUpdateMessage(lobbies));
            }
            catch (Exception ex)
            {
                await m_serverService.SendLobbyServerMessageToClient(connectionId, new FailureResponseMessage(false, ex.Message));
            }
        }

        private async Task OnGetLeaderboardMessageReceived(string connectionId, GetLeaderboardRequestMessage message)
        {
            try
            {
                using var scope = m_serviceScopeFactory.CreateScope();
                var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                var entries = await dbContext.Users
                    .Where(u => u.CompetitiveWins > 0)
                    .OrderByDescending(u => u.CompetitiveWins)
                    .Select(u => new LeaderboardEntryDto(u.UserName ?? string.Empty, u.CompetitiveWins))
                    .ToArrayAsync();
                await m_serverService.SendLobbyServerMessageToClient(connectionId, new GetLeaderboardResponseMessage(true, "OK", entries));
            }
            catch (Exception ex)
            {
                await m_serverService.SendLobbyServerMessageToClient(connectionId, new FailureResponseMessage(false, ex.Message));
            }
        }

        private readonly LobbyRequestMessageHandlerDelegate<CreateLobbyRequestMessage> m_createLobbyRequestMessageHandler;
        private readonly LobbyRequestMessageHandlerDelegate<JoinLobbyRequestMessage> m_joinLobbyRequestMessageHandler;
        private readonly LobbyRequestMessageHandlerDelegate<LeaveLobbyRequestMessage> m_leaveLobbyRequestMessageHandler;
        private readonly LobbyRequestMessageHandlerDelegate<StartGameRequestMessage> m_startGameRequestMessageHandler;
        private readonly LobbyRequestMessageHandlerDelegate<StartMatchmakingRequestMessage> m_startMatchmakingRequestMessageHandler;
        private readonly LobbyRequestMessageHandlerDelegate<StopMatchmakingRequestMessage> m_stopMatchmakingRequestMessageHandler;
        private readonly LobbyRequestMessageHandlerDelegate<SendChatRequestMessage> m_sendChatRequestMessageHandler;
        private readonly LobbyRequestMessageHandlerDelegate<ExitGameRequestMessage> m_exitGameRequestMessageHandler;
        private readonly LobbyRequestMessageHandlerDelegate<GetLobbiesRequestMessage> m_getLobbiesRequestMessageHandler;
        private readonly LobbyRequestMessageHandlerDelegate<GetLeaderboardRequestMessage> m_getLeaderboardRequestMessageHandler;
        private readonly IClientManager m_clientManager;
        private readonly IServerService m_serverService;
        private readonly ILobbyManager m_lobbyManager;
        private readonly IServiceScopeFactory m_serviceScopeFactory;
    }
}
