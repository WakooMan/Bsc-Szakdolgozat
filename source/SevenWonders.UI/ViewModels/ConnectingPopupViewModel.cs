using SevenWonders.Common;
using SevenWonders.UI.Services;
using SevenWonders.UI.ViewModels;
using SevenWondersUI.Services.ConnectionStates;
using System.Windows.Input;

namespace SevenWondersUI.ViewModels
{
    public class ConnectingPopupViewModel : BaseViewModel
    {
        public event EventHandler? OnConnectionFinished;
        public string ConnectingText => "Connecting...";
        public string CancelText => "Cancel";

        public string? ErrorMessage { get; private set; }
        public bool Success { get; set; }
        public bool Cancelled => m_cancelled;
        public ICommand CancelCommand { get; }

        public ConnectingPopupViewModel(IConnectionContext connectionContext, INavigationService navigationService, string userName, string password)
        {
            CancelCommand = new Command(OnCancel);
            m_connectionContext = connectionContext;
            m_navigationService = navigationService;
            m_connectionContext.Username = userName;
            m_connectionContext.Password = password;
            m_cancelled = false;
            Success = false;
        }

        public async void OnOpened()
        {
            IConnectionState connectionState = m_connectionContext.NotConnectedState;
            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(60));

            while (!cts.Token.IsCancellationRequested && connectionState is not ConnectedState && !m_cancelled)
            {
                if (await connectionState.Execute())
                {
                    connectionState = connectionState.NextState();
                }
                else
                {
                    await Task.Delay(5000, cts.Token);
                }
            }

            if (connectionState is ConnectedState && m_connectionContext.Lobbies is not null && !m_cancelled)
            {
                Success = true;
                GameLog.Info("Connection established successfully.");
                OnConnectionFinished?.Invoke(this, EventArgs.Empty);
                await MainThread.InvokeOnMainThreadAsync(async () =>
                {
                    await m_navigationService.NavigateToAsync("//LobbyMainPage", new Dictionary<string, object>
                    {
                        { "Lobbies", m_connectionContext.Lobbies }
                    });
                });
            }
            else
            {
                Success = false;
                GameLog.Error("Failed to establish connection.");
                await OnConnectionClose(connectionState);
            }
        }

        private async Task  OnConnectionClose(IConnectionState connectionState)
        {
            while (connectionState is not NotConnectedState)
            {
                await connectionState.Undo();
                connectionState = connectionState.PreviousState();
            }
            await MainThread.InvokeOnMainThreadAsync(() =>
            {
                ErrorMessage = (!m_cancelled) ? "Nem sikerült csatlakozni a szerverhez." : string.Empty;
                OnConnectionFinished?.Invoke(this, EventArgs.Empty);
            });
        }

        private void OnCancel()
        {
            GameLog.Info("Connection attempt cancelled by user.");
            m_cancelled = true;
        }

        private bool m_cancelled;
        private readonly IConnectionContext m_connectionContext;
        private readonly INavigationService m_navigationService;
    }
}