using SevenWonders.WebClient.Model;
using SevenWonders.WebClient.Model.Services;
using SevenWondersUI.Services;
using WebServer.Contract.Messages.Lobby.ClientMessages;
using WebServer.Contract.Messages.Lobby.ServerMessages;

namespace SevenWondersUI.ViewModels
{
    [QueryProperty(nameof(AuthToken), "AuthToken")]
    [QueryProperty(nameof(UserName), "UserName")]
    public class ConnectPageViewModel : BaseViewModel, IMessageHandler
    {
        public string AuthToken { get; set; }
        public string UserName { get; set; }

        public ConnectPageViewModel(INavigationService navigationService, IClientHubService clientHubService)
        {
            m_navigationService = navigationService;
            m_clientHubService = clientHubService;
            m_lobbyUpdateMessageHandlerDelegate = new LobbyResponseMessageHandlerDelegate<LobbyUpdateMessage>(OnLobbyUpdateMessageReceived);
            AuthToken = string.Empty;
            UserName = string.Empty;
        }

        public void Register(IMessageRegisterer registerer)
        {
            registerer.Register(m_lobbyUpdateMessageHandlerDelegate);
        }

        public void Unregister(IMessageRegisterer registerer)
        {
            registerer.Unregister(m_lobbyUpdateMessageHandlerDelegate);
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

        private readonly LobbyResponseMessageHandlerDelegate<LobbyUpdateMessage> m_lobbyUpdateMessageHandlerDelegate;
        private readonly INavigationService m_navigationService;
        private readonly IClientHubService m_clientHubService;
    }
}
