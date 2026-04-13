using SevenWondersUI.Services;

namespace SevenWondersUI.ViewModels
{
    [QueryProperty(nameof(AuthToken), "AuthToken")]
    public class ConnectPageViewModel : BaseViewModel
    {
        public string AuthToken { get; set; }

        public ConnectPageViewModel(INavigationService navigationService, IClientHubService clientHubService)
        {
            m_navigationService = navigationService;
            m_clientHubService = clientHubService;
            AuthToken = string.Empty;
        }

        public async Task ConnectToServer()
        {
            try
            {
                int retry = 0;
                while (string.IsNullOrEmpty(AuthToken) && retry < 300)
                {
                    await Task.Delay(100);
                    retry++;
                }

                if (string.IsNullOrEmpty(AuthToken))
                {
                    throw new InvalidOperationException("Authorization token is not initialized in 30 seconds!");
                }

                await m_clientHubService.Connect(AuthToken);
                await m_navigationService.NavigateToAsync("//LobbyMainPage");
            }
            catch (Exception ex)
            {
                await m_navigationService.NavigateToAsync("//LoginPage");
            }
        }

        private readonly INavigationService m_navigationService;
        private readonly IClientHubService m_clientHubService;
    }
}
