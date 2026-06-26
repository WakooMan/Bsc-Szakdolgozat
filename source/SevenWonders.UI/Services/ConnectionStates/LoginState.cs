using SevenWonders.Common;
using SevenWonders.Web.Client.Model.Services;
using SevenWonders.Web.Server.Contract;

namespace SevenWondersUI.Services.ConnectionStates
{
    public class LoginState : IConnectionState
    {
        public LoginState(IConnectionContext connectionContext, IAuthService authService)
        {
            m_connectionContext = connectionContext;
            m_authService = authService;
            m_isLoggedIn = false;
        }

        public async Task<bool> Execute()
        {
            LoginResponse? loginResponse = await m_authService.LoginAsync(m_connectionContext.Username, m_connectionContext.Password);
            if(loginResponse is not null && loginResponse.Success)
            {
                m_connectionContext.AuthToken = loginResponse.Token;
                m_isLoggedIn = true;
                return m_isLoggedIn;
            }

            return false;
        }

        public async Task Undo()
        {
            m_connectionContext.AuthToken = string.Empty;
            if (m_isLoggedIn)
            {
                try
                {
                    await m_authService.LogoutAsync();
                    m_isLoggedIn = false;
                }
                catch (Exception ex)
                {
                    GameLog.Error($"Failed to logout: {ex.Message}");
                }
            }
        }

        public IConnectionState NextState()
        {
            return m_connectionContext.ConnectToSignalRState;
        }

        public IConnectionState PreviousState()
        {
            return m_connectionContext.NotConnectedState;
        }

        private readonly IAuthService m_authService;
        private readonly IConnectionContext m_connectionContext;
        private bool m_isLoggedIn;
    }
}
