using SevenWonders.Web.Client.Model;
using SevenWonders.Web.Client.Model.Services;
using SevenWonders.UI.Services;
using SevenWonders.Web.Server.Contract.Messages.Lobby.ClientMessages;
using SevenWonders.Web.Server.Contract.Messages.Lobby.ServerMessages;

namespace SevenWonders.UI.ViewModels
{
    [QueryProperty(nameof(AuthToken), "AuthToken")]
    [QueryProperty(nameof(UserName), "UserName")]
    public class ConnectPageViewModel : BaseViewModel, IMessageHandler
    {
        public string AuthToken { get; set; }
        public string UserName { get; set; }

        public ConnectPageViewModel(INavigationService navigationService, IClientHubService clientHubService, IAuthService authService)
        {
            m_navigationService = navigationService;
            m_clientHubService = clientHubService;
            m_authService = authService;
            m_lobbyUpdateMessageHandlerDelegate = new LobbyResponseMessageHandlerDelegate<LobbyUpdateMessage>(OnLobbyUpdateMessageReceived);
            m_failureResponseMessageHandlerDelegate = new LobbyResponseMessageHandlerDelegate<FailureResponseMessage>(OnFailureResponseMessageReceived);
            AuthToken = string.Empty;
            UserName = string.Empty;
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

        public async Task ConnectToServer()
        {
            try
            {
                int retry = 0;
                while ((string.IsNullOrEmpty(AuthToken) || string.IsNullOrEmpty(UserName)) && retry < 300)
                {
                    await Task.Delay(100);
                    retry++;
                }

                if (string.IsNullOrEmpty(AuthToken) || string.IsNullOrEmpty(UserName))
                {
                    throw new InvalidOperationException("Authorization token or username is not initialized in 30 seconds!");
                }

                await m_clientHubService.Connect(UserName, AuthToken);
                await m_clientHubService.InvokeLobbyCommand(new GetLobbiesRequestMessage());
            }
            catch (Exception)
            {
                await m_navigationService.NavigateToAsync("//LoginPage");
            }
        }

        private async Task<bool> OnLobbyUpdateMessageReceived(LobbyUpdateMessage message)
        {
            if (message.Success)
            {
                await MainThread.InvokeOnMainThreadAsync(async () =>
                {
                    await m_navigationService.NavigateToAsync("//LobbyMainPage", new Dictionary<string, object>
                    {
                        { "Lobbies", message.Lobbies }
                    });
                });
            }
            return message.Success;
        }

        private async Task<bool> OnFailureResponseMessageReceived(FailureResponseMessage message)
        {
            await MainThread.InvokeOnMainThreadAsync(async () =>
            {
                await m_clientHubService.Disconnect();
                await m_authService.LogoutAsync();
                await m_navigationService.NavigateToAsync("//LoginPage");
            });
            return false;
        }

        private readonly LobbyResponseMessageHandlerDelegate<LobbyUpdateMessage> m_lobbyUpdateMessageHandlerDelegate;
        private readonly LobbyResponseMessageHandlerDelegate<FailureResponseMessage> m_failureResponseMessageHandlerDelegate;
        private readonly INavigationService m_navigationService;
        private readonly IClientHubService m_clientHubService;
        private readonly IAuthService m_authService;
    }
}
