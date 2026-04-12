using Microsoft.AspNetCore.SignalR.Client;
using SevenWondersUI.Services;

namespace SevenWondersUI.ViewModels
{
    [QueryProperty(nameof(AuthToken), "AuthToken")]
    public class ConnectPageViewModel : BaseViewModel
    {
        public string AuthToken { get; set; }

        public ConnectPageViewModel(INavigationService navigationService)
        {
            m_navigationService = navigationService;
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

                var hubConnection = new HubConnectionBuilder()
                .WithUrl("https://localhost:7206/serverhub", options =>
                {
                    options.AccessTokenProvider = () => Task.FromResult((string?)AuthToken);
                })
                .WithAutomaticReconnect()
                .Build();

                hubConnection.HandshakeTimeout = TimeSpan.FromSeconds(15);
                hubConnection.ServerTimeout = TimeSpan.FromSeconds(30);

                await hubConnection.StartAsync();
                await m_navigationService.NavigateToAsync("//LobbyMainPage");
            }
            catch (Exception ex)
            {
                await m_navigationService.NavigateToAsync("//LoginPage");
            }
        }

        private readonly INavigationService m_navigationService;
    }
}
