using SevenWonders.Common;
using SevenWonders.Web.Client.Model.Services;

namespace SevenWondersUI.Services.ConnectionStates
{
    public class ConnectToSignalRState : IConnectionState
    {
        public ConnectToSignalRState(IConnectionContext connectionContext, IClientHubService clientHubService)
        {
            m_connectionContext = connectionContext;
            m_clientHubService = clientHubService;
            m_isConnected = false;
        }
        public async Task<bool> Execute()
        {
            if (string.IsNullOrEmpty(m_connectionContext.AuthToken) || string.IsNullOrEmpty(m_connectionContext.Username))
            {
                return false;
            }

            try
            {
                await m_clientHubService.Connect(m_connectionContext.Username, m_connectionContext.AuthToken);
                m_isConnected = true;
                return m_isConnected;
            }
            catch (Exception ex)
            {
                GameLog.Error($"Failed to connect to SignalR: {ex.Message}");
                m_isConnected = false;
                return false;
            }
        }

        public async Task Undo()
        {
            if (m_isConnected)
            {
                try
                {
                    await m_clientHubService.Disconnect();
                    m_isConnected = false;
                }
                catch (Exception ex)
                {
                    GameLog.Error($"Failed to disconnect from SignalR: {ex.Message}");
                }
            }
        }

        public IConnectionState NextState()
        {
            return m_connectionContext.ReceiveLobbiesState;
        }

        public IConnectionState PreviousState()
        {
            return m_connectionContext.LoginState;
        }

        private readonly IConnectionContext m_connectionContext;
        private readonly IClientHubService m_clientHubService;
        private bool m_isConnected;
    }
}
