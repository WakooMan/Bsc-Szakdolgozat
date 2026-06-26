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
    public class ConnectingPopupViewModel : BaseViewModel, IMessageHandler
    {
        public event EventHandler? OnConnectionFinished;
        public string ConnectingText => "Connecting...";
        public string CancelText => "Cancel";

        public string? ErrorMessage { get; private set; }
        public bool Success => (m_lobbyUpdateTcs is not null && m_lobbyUpdateTcs.Task.IsCompleted) ? m_lobbyUpdateTcs.Task.Result : false;
        public bool Finished => m_lobbyUpdateTcs?.Task.IsCompleted ?? false;
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
        }

        public async void OnOpened()
        {
            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(60));
            m_lobbyUpdateTcs = new TaskCompletionSource<bool>();

            cts.Token.Register(() => m_lobbyUpdateTcs.TrySetResult(false));

            while (!cts.Token.IsCancellationRequested && !Finished && !m_cancelled && !m_failedExplicitly)
            {
                try
                {
                    LoginResponse? result = await m_authService.LoginAsync(m_userName, m_password);
                    if (result is not null && result.Success && !string.IsNullOrEmpty(result.Token))
                    {
                        await ConnectToServer(result.Token);

                        var delayTask = Task.Delay(TimeSpan.FromSeconds(5), cts.Token);
                        var completedTask = await Task.WhenAny(m_lobbyUpdateTcs.Task, delayTask);

                        if (completedTask == m_lobbyUpdateTcs.Task)
                        {
                            break;
                        }
                    }
                    else
                    {
                        await Task.Delay(TimeSpan.FromSeconds(5), cts.Token);
                    }
                }
                catch (OperationCanceledException)
                {
                    break;
                }
                catch (Exception ex)
                {
                    GameLog.Error($"Exception happened during connection retry: {ex.Message}");
                    GameLog.Error($"StackTrace: {ex.StackTrace ?? string.Empty}");

                    try
                    {
                        await Task.Delay(TimeSpan.FromSeconds(5), cts.Token);
                    }
                    catch (OperationCanceledException) { break; }
                }
            }

            if (!Success && !m_cancelled && !m_failedExplicitly)
            {
                GameLog.Error("Connection timeout (60s) reached. Treating as failure response.");
                await TryDisconnectAsync();
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
            if (string.IsNullOrEmpty(authToken) || string.IsNullOrEmpty(m_userName))
            {
                throw new InvalidOperationException("Authorization token or username is missing!");
            }

            await m_clientHubService.Connect(m_userName, authToken);
            await m_clientHubService.InvokeLobbyCommand(new GetLobbiesRequestMessage());
        }

        private async Task<bool> OnLobbyUpdateMessageReceived(LobbyUpdateMessage message)
        {
            if (message.Success)
            {
                m_lobbyUpdateTcs?.TrySetResult(true);
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
                await TryDisconnectAsync();
            }
            return message.Success;
        }

        private async Task<bool> OnFailureResponseMessageReceived(FailureResponseMessage message)
        {
            GameLog.Error("Server sent failure response message: Logging out.");
            m_failedExplicitly = true;
            await TryDisconnectAsync();
            return false;
        }

        private async Task TryDisconnectAsync()
        {
            ErrorMessage = "Connecting to the server failed.";
            m_lobbyUpdateTcs?.TrySetResult(false);
            await m_clientHubService.Disconnect();
            await m_authService.LogoutAsync();
            await MainThread.InvokeOnMainThreadAsync(async () =>
            {
                OnConnectionFinished?.Invoke(this, new EventArgs());
            });
        }

        private void OnCancel()
        {
            m_cancelled = true;
            m_lobbyUpdateTcs?.TrySetResult(false);
            TryDisconnectAsync().GetAwaiter().GetResult();

        }

        private readonly LobbyResponseMessageHandlerDelegate<LobbyUpdateMessage> m_lobbyUpdateMessageHandlerDelegate;
        private readonly LobbyResponseMessageHandlerDelegate<FailureResponseMessage> m_failureResponseMessageHandlerDelegate;
        private readonly string m_userName;
        private readonly string m_password;

        private bool m_cancelled;
        private bool m_failedExplicitly;
        private TaskCompletionSource<bool>? m_lobbyUpdateTcs;

        private readonly INavigationService m_navigationService;
        private readonly IClientHubService m_clientHubService;
        private readonly IAuthService m_authService;
    }
}