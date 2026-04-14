using WebServer.Contract.Messages.Lobby.ClientMessages;
using WebServer.Model.Client;

namespace WebServer.Model.MessageHandling
{
    public class LobbyMessageHandlers : ILobbyMessageHandlers
    {
        public LobbyMessageHandlers(IClientManager clientManager)
        {
            m_clientManager = clientManager;
            m_createLobbyRequestMessageHandler = new LobbyRequestMessageHandlerDelegate<CreateLobbyRequestMessage>(OnCreateLobbyMessageReceived);
            m_joinLobbyRequestMessageHandler = new LobbyRequestMessageHandlerDelegate<JoinLobbyRequestMessage>(OnJoinLobbyMessageReceived);
            m_leaveLobbyRequestMessageHandler = new LobbyRequestMessageHandlerDelegate<LeaveLobbyRequestMessage>(OnLeaveLobbyMessageReceived);
            m_startGameRequestMessageHandler = new LobbyRequestMessageHandlerDelegate<StartGameRequestMessage>(OnStartGameMessageReceived);
            m_startMatchmakingRequestMessageHandler = new LobbyRequestMessageHandlerDelegate<StartMatchmakingRequestMessage>(OnStartMatchmakingMessageReceived);
            m_stopMatchmakingRequestMessageHandler = new LobbyRequestMessageHandlerDelegate<StopMatchmakingRequestMessage>(OnStopMatchmakingMessageReceived);
            m_sendChatRequestMessageHandler = new LobbyRequestMessageHandlerDelegate<SendChatRequestMessage>(OnSendChatMessageReceived);
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
                // Handle exception if needed
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
                // Handle exception if needed
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
                // Handle exception if needed
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
                // Handle exception if needed
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
                // Handle exception if needed
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
                // Handle exception if needed
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
                // Handle exception if needed
            }
        }

        private readonly LobbyRequestMessageHandlerDelegate<CreateLobbyRequestMessage> m_createLobbyRequestMessageHandler;
        private readonly LobbyRequestMessageHandlerDelegate<JoinLobbyRequestMessage> m_joinLobbyRequestMessageHandler;
        private readonly LobbyRequestMessageHandlerDelegate<LeaveLobbyRequestMessage> m_leaveLobbyRequestMessageHandler;
        private readonly LobbyRequestMessageHandlerDelegate<StartGameRequestMessage> m_startGameRequestMessageHandler;
        private readonly LobbyRequestMessageHandlerDelegate<StartMatchmakingRequestMessage> m_startMatchmakingRequestMessageHandler;
        private readonly LobbyRequestMessageHandlerDelegate<StopMatchmakingRequestMessage> m_stopMatchmakingRequestMessageHandler;
        private readonly LobbyRequestMessageHandlerDelegate<SendChatRequestMessage> m_sendChatRequestMessageHandler;
        private readonly IClientManager m_clientManager;
    }
}
