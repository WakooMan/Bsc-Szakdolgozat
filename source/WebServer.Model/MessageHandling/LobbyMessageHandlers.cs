using Microsoft.AspNetCore.SignalR;
using WebServer.Contract.DataTransferObjects;
using WebServer.Contract.Messages.Lobby.ClientMessages;
using WebServer.Contract.Messages.Lobby.ServerMessages;
using WebServer.Model.Client;
using WebServer.Model.Lobby;

namespace WebServer.Model.MessageHandling
{
    public class LobbyMessageHandlers : ILobbyMessageHandlers
    {
        public LobbyMessageHandlers(IClientManager clientManager, ILobbyCodeGenerator lobbyCodeGenerator)
        {
            m_clientManager = clientManager;
            m_lobbyCodeGenerator = lobbyCodeGenerator;
            m_createLobbyRequestMessageHandler = new LobbyRequestMessageHandlerDelegate<CreateLobbyRequestMessage>(OnCreateLobbyMessageReceived);
            m_joinLobbyRequestMessageHandler = new LobbyRequestMessageHandlerDelegate<JoinLobbyRequestMessage>(OnJoinLobbyMessageReceived);
            m_leaveLobbyRequestMessageHandler = new LobbyRequestMessageHandlerDelegate<LeaveLobbyRequestMessage>(OnLeaveLobbyMessageReceived);
            m_startGameRequestMessageHandler = new LobbyRequestMessageHandlerDelegate<StartGameRequestMessage>(OnStartGameMessageReceived);
            m_startMatchmakingRequestMessageHandler = new LobbyRequestMessageHandlerDelegate<StartMatchmakingRequestMessage>(OnStartMatchmakingMessageReceived);
            m_stopMatchmakingRequestMessageHandler = new LobbyRequestMessageHandlerDelegate<StopMatchmakingRequestMessage>(OnStopMatchmakingMessageReceived);
        }

        public void Register(IMessageRegisterer registerer)
        {
            registerer.Register(m_createLobbyRequestMessageHandler);
            registerer.Register(m_joinLobbyRequestMessageHandler);
            registerer.Register(m_leaveLobbyRequestMessageHandler);
            registerer.Register(m_startGameRequestMessageHandler);
            registerer.Register(m_startMatchmakingRequestMessageHandler);
            registerer.Register(m_stopMatchmakingRequestMessageHandler);
        }

        public void Unregister(IMessageRegisterer registerer)
        {
            registerer.Unregister(m_createLobbyRequestMessageHandler);
            registerer.Unregister(m_joinLobbyRequestMessageHandler);
            registerer.Unregister(m_leaveLobbyRequestMessageHandler);
            registerer.Unregister(m_startGameRequestMessageHandler);
            registerer.Unregister(m_startMatchmakingRequestMessageHandler);
            registerer.Unregister(m_stopMatchmakingRequestMessageHandler);
        }

        private async Task<LobbyServerMessage> OnCreateLobbyMessageReceived(Hub hub, string connectionId, CreateLobbyRequestMessage requestMessage)
        {
            try
            {
                IPlayerClient playerClient = m_clientManager.GetClient(connectionId);
                string code = m_lobbyCodeGenerator.GenerateUniqueCode();
                ILobby lobby = playerClient.CreateLobby(code, requestMessage.Name);
                return await Task.FromResult(new CreateLobbyResponseMessage(true, "Success", lobby.ToDto()));
            } 
            catch (Exception ex)
            {
                return await Task.FromResult(new CreateLobbyResponseMessage(false, ex.Message, new LobbyDto()));
            }
        }

        private async Task<LobbyServerMessage> OnStopMatchmakingMessageReceived(Hub hub, string connectionId, StopMatchmakingRequestMessage message)
        {
            try
            {
                IPlayerClient playerClient = m_clientManager.GetClient(connectionId);
                playerClient.ExitMatchmaking();
                return await Task.FromResult(new StopMatchmakingResponseMessage(true, "OK"));
            }
            catch (Exception ex)
            {
                return await Task.FromResult(new StopMatchmakingResponseMessage(false, ex.Message));
            }
        }

        private async Task<LobbyServerMessage> OnStartMatchmakingMessageReceived(Hub hub, string connectionId, StartMatchmakingRequestMessage message)
        {
            try
            {
                IPlayerClient playerClient = m_clientManager.GetClient(connectionId);
                playerClient.StartMatchmaking();
                return await Task.FromResult(new StartMatchmakingResponseMessage(true, "OK"));
            }
            catch (Exception ex)
            {
                return await Task.FromResult(new StartMatchmakingResponseMessage(false, ex.Message));
            }
        }

        private async Task<LobbyServerMessage> OnLeaveLobbyMessageReceived(Hub hub, string connectionId, LeaveLobbyRequestMessage message)
        {
            try
            {
                IPlayerClient playerClient = m_clientManager.GetClient(connectionId);
                playerClient.LeaveLobby();
                return await Task.FromResult(new LeaveLobbyResponseMessage(true, "OK"));
            }
            catch (Exception ex)
            {
                return await Task.FromResult(new LeaveLobbyResponseMessage(false, ex.Message));
            }
        }

        private async Task<LobbyServerMessage> OnStartGameMessageReceived(Hub hub, string connectionId, StartGameRequestMessage message)
        {
            try
            {
                IPlayerClient playerClient = m_clientManager.GetClient(connectionId);
                playerClient.StartGame();
                return await Task.FromResult(new StartGameResponseMessage(true, "OK"));
            }
            catch (Exception ex)
            {
                return await Task.FromResult(new StartGameResponseMessage(false, ex.Message));
            }
        }

        private async Task<LobbyServerMessage> OnJoinLobbyMessageReceived(Hub hub, string connectionId, JoinLobbyRequestMessage message)
        {
            try
            {
                IPlayerClient playerClient = m_clientManager.GetClient(connectionId);
                ILobby lobby = playerClient.JoinLobby(message.Code);
                return await Task.FromResult(new JoinLobbyResponseMessage(true, "Success", lobby.ToDto()));
            }
            catch (Exception ex)
            {
                return await Task.FromResult(new JoinLobbyResponseMessage(false, ex.Message, new LobbyDto()));
            }
        }

        private readonly LobbyRequestMessageHandlerDelegate<CreateLobbyRequestMessage> m_createLobbyRequestMessageHandler;
        private readonly LobbyRequestMessageHandlerDelegate<JoinLobbyRequestMessage> m_joinLobbyRequestMessageHandler;
        private readonly LobbyRequestMessageHandlerDelegate<LeaveLobbyRequestMessage> m_leaveLobbyRequestMessageHandler;
        private readonly LobbyRequestMessageHandlerDelegate<StartGameRequestMessage> m_startGameRequestMessageHandler;
        private readonly LobbyRequestMessageHandlerDelegate<StartMatchmakingRequestMessage> m_startMatchmakingRequestMessageHandler;
        private readonly LobbyRequestMessageHandlerDelegate<StopMatchmakingRequestMessage> m_stopMatchmakingRequestMessageHandler;
        private readonly IClientManager m_clientManager;
        private readonly ILobbyCodeGenerator m_lobbyCodeGenerator;
    }
}
