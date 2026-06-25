using SevenWonders.Common;
using SevenWonders.UI.Services;
using SevenWonders.UI.ViewModels;
using SevenWonders.Web.Client.Model;
using SevenWonders.Web.Client.Model.Services;
using SevenWonders.Web.Server.Contract;
using SevenWonders.Web.Server.Contract.Messages.Lobby.ClientMessages;
using SevenWonders.Web.Server.Contract.Messages.Lobby.ServerMessages;
using System.Windows.Input;

namespace SevenWondersUI.ViewModels
{
    public class ConnectingPopupViewModel: BaseViewModel, IMessageHandler
    {
        public event EventHandler? OnConnectionFinished;
        public string ConnectingText => "Connecting...";
        public string CancelText => "Cancel";
        public bool Success => m_success;
        public bool Cancelled => m_cancelled;
        public ICommand CancelCommand { get; }

        public ConnectingPopupViewModel(INavigationService navigationService, IClientHubService clientHubService, IAuthService authService, string userName, string password)
        {
            CancelCommand = new Command(OnCancel);
            m_navigationService = navigationService;
            m_clientHubService = clientHubService;
            m_authService = authService;
            m_lobbyUpdateMessageHandlerDelegate = new LobbyResponseMessageHandlerDelegate<LobbyUpdateMessage>(OnLobbyUpdateMessageReceived);
            m_failureResponseMessageHandlerDelegate = new LobbyResponseMessageHandlerDelegate<FailureResponseMessage>(OnFailureResponseMessageReceived);
            m_userName = userName;
            m_password = password;
            m_success = false;
        }

        public async void OnOpened()
        {
            LoginResponse? result = await m_authService.LoginAsync(m_userName, m_password);
            if (result is not null && result.Success)
            {
                await ConnectToServer(result.Token);
            }
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

        public async Task ConnectToServer(string authToken)
        {
            try
            {
                int retry = 0;
                while ((string.IsNullOrEmpty(authToken) || string.IsNullOrEmpty(m_userName)) && retry < 300)
                {
                    await Task.Delay(100);
                    retry++;
                }

                if (string.IsNullOrEmpty(authToken) || string.IsNullOrEmpty(m_userName))
                {
                    throw new InvalidOperationException("Authorization token or username is not initialized in 30 seconds!");
                }

                await m_clientHubService.Connect(m_userName, authToken);
                await m_clientHubService.InvokeLobbyCommand(new GetLobbiesRequestMessage());
            }
            catch (Exception ex)
            {
                GameLog.Error($"Exception happened during connecting to the server: {ex.Message}");
                GameLog.Error($"StackTrace: {ex.StackTrace ?? string.Empty}");
                OnConnectionFinished?.Invoke(this, new EventArgs());
            }
        }

        private async Task<bool> OnLobbyUpdateMessageReceived(LobbyUpdateMessage message)
        {
            if (message.Success)
            {
                m_success = true;
                OnConnectionFinished?.Invoke(this, new EventArgs());
                await MainThread.InvokeOnMainThreadAsync(async () =>
                {
                    await m_navigationService.NavigateToAsync("//LobbyMainPage", new Dictionary<string, object>
                    {
                        { "Lobbies", message.Lobbies }
                    });
                });
            }
            else
            {
                OnConnectionFinished?.Invoke(this, new EventArgs());
            }
            return message.Success;
        }

        private async Task<bool> OnFailureResponseMessageReceived(FailureResponseMessage message)
        {
            GameLog.Error("Server sent failure response message: Logging out.");
            await MainThread.InvokeOnMainThreadAsync(async () =>
            {
                await m_clientHubService.Disconnect();
                await m_authService.LogoutAsync();
                OnConnectionFinished?.Invoke(this, new EventArgs());
            });
            return false;
        }

        private void OnCancel()
        {
            m_cancelled = true;
        }

        private readonly LobbyResponseMessageHandlerDelegate<LobbyUpdateMessage> m_lobbyUpdateMessageHandlerDelegate;
        private readonly LobbyResponseMessageHandlerDelegate<FailureResponseMessage> m_failureResponseMessageHandlerDelegate;
        private readonly string m_userName;
        private readonly string m_password;
        private bool m_success;
        private bool m_cancelled;
        private readonly INavigationService m_navigationService;
        private readonly IClientHubService m_clientHubService;
        private readonly IAuthService m_authService;
    }
}
