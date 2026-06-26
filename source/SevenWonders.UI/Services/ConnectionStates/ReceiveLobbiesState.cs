using SevenWonders.Web.Client.Model;
using SevenWonders.Web.Client.Model.Services;
using SevenWonders.Web.Server.Contract.Messages.Lobby.ClientMessages;
using SevenWonders.Web.Server.Contract.Messages.Lobby.ServerMessages;

namespace SevenWondersUI.Services.ConnectionStates
{
    public class ReceiveLobbiesState : IConnectionState, IMessageHandler
    {
        public ReceiveLobbiesState(IConnectionContext connectionContext, IClientHubService clientHubService, IClientMessageDispatcher clientMessageDispatcher)
        {
            m_connectionContext = connectionContext;
            m_clientHubService = clientHubService;
            m_clientMessageDispatcher = clientMessageDispatcher;
            m_lobbyUpdateMessageHandlerDelegate = new LobbyResponseMessageHandlerDelegate<LobbyUpdateMessage>(OnLobbyUpdateMessageReceived);
            m_failureResponseMessageHandlerDelegate = new LobbyResponseMessageHandlerDelegate<FailureResponseMessage>(OnFailureResponseMessageReceived);
            m_waitingForResponse = false;
            m_isRegistered = false;
        }

        public void Register(IMessageRegisterer registerer)
        {
            registerer.Register(m_lobbyUpdateMessageHandlerDelegate);
            registerer.Register(m_failureResponseMessageHandlerDelegate);
        }

        public void Unregister(IMessageRegisterer registerer)
        {
            registerer.Unregister(m_lobbyUpdateMessageHandlerDelegate);
            registerer.Unregister(m_failureResponseMessageHandlerDelegate);
        }

        public async Task<bool> Execute()
        {
            if (!m_isRegistered)
            {
                m_clientMessageDispatcher.RegisterHandler(this);
                m_isRegistered = true;
            }

            if (!m_waitingForResponse && m_connectionContext.Lobbies is null)
            {
                await m_clientHubService.InvokeLobbyCommand(new GetLobbiesRequestMessage());
                m_waitingForResponse = true;
            }
            else if (!m_waitingForResponse && m_connectionContext.Lobbies is not null)
            {
                m_clientMessageDispatcher.UnregisterHandler(this);
                m_isRegistered = false;
                return true;
            }

            return false;
        }

        public Task Undo()
        {
            if (m_isRegistered)
            {
                m_clientMessageDispatcher.UnregisterHandler(this);
                m_isRegistered = false;
            }
            m_waitingForResponse = false;
            m_connectionContext.Lobbies = null;
            return Task.CompletedTask;
        }

        public IConnectionState NextState()
        {
            return m_connectionContext.ConnectedState;
        }

        public IConnectionState PreviousState()
        {
            return m_connectionContext.ConnectToSignalRState;
        }

        private Task<bool> OnLobbyUpdateMessageReceived(LobbyUpdateMessage message)
        {
            if (m_waitingForResponse)
            {
                if (message.Success)
                {
                    m_connectionContext.Lobbies = message.Lobbies;
                }

                m_waitingForResponse = false;
                return Task.FromResult(message.Success);
            }
            return Task.FromResult(false);
        }

        private Task<bool> OnFailureResponseMessageReceived(FailureResponseMessage message)
        {
            m_waitingForResponse = false;
            return Task.FromResult(false);
        }

        private bool m_waitingForResponse;
        private bool m_isRegistered;
        private readonly IConnectionContext m_connectionContext;
        private readonly IClientHubService m_clientHubService;
        private readonly IClientMessageDispatcher m_clientMessageDispatcher;
        private readonly LobbyResponseMessageHandlerDelegate<LobbyUpdateMessage> m_lobbyUpdateMessageHandlerDelegate;
        private readonly LobbyResponseMessageHandlerDelegate<FailureResponseMessage> m_failureResponseMessageHandlerDelegate;
    }
}
