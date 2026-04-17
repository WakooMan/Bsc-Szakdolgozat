using SevenWonders.Presenter;
using SevenWonders.WebClient.Model;
using SevenWonders.WebClient.Model.Services;

namespace SevenWondersUI.Services.Factories
{
    public class GameOverHandlerFactory : IGameOverHandlerFactory
    {
        public GameOverHandlerFactory(IClientHubService clientHubService, INavigationService navigationService, IClientMessageDispatcher clientMessageDispatcher)
        {
            m_clientHubService = clientHubService;
            m_navigationService = navigationService;
            m_clientMessageDispatcher = clientMessageDispatcher;
        }

        public IGameOverHandler Create(bool isMultiplayer)
        {
            if (isMultiplayer)
            {
                return new MultiplayerGameOverHandler(m_clientHubService, m_navigationService, m_clientMessageDispatcher);
            }
            else
            {
                return new SingleplayerGameOverHandler(m_navigationService);
            }
        }

        private readonly IClientHubService m_clientHubService;
        private readonly INavigationService m_navigationService;
        private readonly IClientMessageDispatcher m_clientMessageDispatcher;
    }
}
