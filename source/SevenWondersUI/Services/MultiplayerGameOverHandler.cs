using SevenWonders.Presenter;
using SevenWonders.WebClient.Model;
using SevenWonders.WebClient.Model.Services;
using WebServer.Contract.Messages.Lobby.ClientMessages;
using WebServer.Contract.Messages.Lobby.ServerMessages;

namespace SevenWondersUI.Services
{
    public class MultiplayerGameOverHandler : IGameOverHandler, IMessageHandler
    {
        public MultiplayerGameOverHandler(IClientHubService clientHubService, INavigationService navigationService)
        {
            m_clientHubService = clientHubService;
            m_navigationService = navigationService;
            m_lobbyResponseMessageHandlerDelegate = new LobbyResponseMessageHandlerDelegate<ExitGameResponseMessage>(HandleExitGameResponse);
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

        private readonly IClientHubService m_clientHubService;
        private readonly INavigationService m_navigationService;
        private readonly LobbyResponseMessageHandlerDelegate<ExitGameResponseMessage> m_lobbyResponseMessageHandlerDelegate;
    }
}
