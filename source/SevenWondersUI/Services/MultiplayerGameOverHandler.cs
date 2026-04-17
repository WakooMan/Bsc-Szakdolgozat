using SevenWonders.Presenter;
using SevenWonders.WebClient.Model;
using SevenWonders.WebClient.Model.Services;
using WebServer.Contract.Messages.Lobby.ClientMessages;
using WebServer.Contract.Messages.Lobby.ServerMessages;

namespace SevenWondersUI.Services
{
    public class MultiplayerGameOverHandler : IGameOverHandler, IMessageHandler, IDisposable
    {
        public MultiplayerGameOverHandler(IClientHubService clientHubService, INavigationService navigationService, IClientMessageDispatcher clientMessageDispatcher)
        {
            m_clientHubService = clientHubService;
            m_navigationService = navigationService;
            m_clientMessageDispatcher = clientMessageDispatcher;
            m_lobbyResponseMessageHandlerDelegate = new LobbyResponseMessageHandlerDelegate<ExitGameResponseMessage>(HandleExitGameResponse);
            m_clientMessageDispatcher.RegisterHandler(this);
        }

        public async Task OnGameOver()
        {
            await m_clientHubService.InvokeLobbyCommand(new ExitGameRequestMessage());
        }

        public void Register(IMessageRegisterer registerer)
        {
            registerer.Register(m_lobbyResponseMessageHandlerDelegate);
        }

        public void Unregister(IMessageRegisterer registerer)
        {
            registerer.Unregister(m_lobbyResponseMessageHandlerDelegate);
        }

        private async Task<bool> HandleExitGameResponse(ExitGameResponseMessage message)
        {
            if (message.Success)
            {
                await MainThread.InvokeOnMainThreadAsync(async () =>
                {
                    await m_navigationService.NavigateToAsync("//LobbyMainPage", new Dictionary<string, object>() { { "Lobbies", message.Lobbies } });
                });
            }
            return message.Success;
        }

        public void Dispose()
        {
            m_clientMessageDispatcher.UnregisterHandler(this);
        }

        private readonly IClientHubService m_clientHubService;
        private readonly INavigationService m_navigationService;
        private readonly IClientMessageDispatcher m_clientMessageDispatcher;
        private readonly LobbyResponseMessageHandlerDelegate<ExitGameResponseMessage> m_lobbyResponseMessageHandlerDelegate;
    }
}
