using SevenWonders.WebClient.Model.Services;
using SevenWondersUI.Services;

namespace SevenWondersUI.ViewModels
{
    [QueryProperty(nameof(AuthToken), "AuthToken")]
    [QueryProperty(nameof(UserName), "UserName")]
    public class ConnectPageViewModel : BaseViewModel
    {
        public string AuthToken { get; set; }
        public string UserName { get; set; }

        public ConnectPageViewModel(INavigationService navigationService, IClientHubService clientHubService)
        {
            m_navigationService = navigationService;
            m_clientHubService = clientHubService;
            AuthToken = string.Empty;
            UserName = string.Empty;
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
                await m_navigationService.NavigateToAsync("//LobbyMainPage");
            }
            catch (Exception)
            {
                await m_navigationService.NavigateToAsync("//LoginPage");
            }
        }

        private readonly INavigationService m_navigationService;
        private readonly IClientHubService m_clientHubService;
    }
}
