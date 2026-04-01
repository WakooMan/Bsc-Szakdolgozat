using GameLogic.Events;
using SevenWonders.GameEngine;
using SevenWonders.Presenter.Connectors;
using SevenWonders.Presenter.Connectors.Cards;
using SevenWonders.Presenter.Connectors.Wonders;

namespace SevenWonders.Presenter.Presenters.Factories
{
    public class PresenterFactory : IPresenterFactory
    {
        public PresenterFactory(ICardConnector cardConnector, IWonderConnector wonderConnector, IGameEngineReceiver gameEngineReceiver, IEventManager eventManager, IPlayerCardHandlerFactory playerCardHandlerFactory, ISceneManager sceneManager)
        {
            m_cardConnector = cardConnector;
            m_wonderConnector = wonderConnector;
            m_gameEngineReceiver = gameEngineReceiver;
            m_eventManager = eventManager;
            m_playerCardHandlerFactory = playerCardHandlerFactory;
            m_sceneManager = sceneManager;
        }

        public IPresenter CreateCardPresenter()
        {
            return new CardPresenter(m_cardConnector, m_gameEngineReceiver, m_eventManager, m_playerCardHandlerFactory, m_sceneManager);
        }

        public IPresenter CreateWonderPresenter()
        {
            return new WonderPresenter(m_wonderConnector, m_gameEngineReceiver, m_eventManager);
        }

        public IPresenter CreatePlayer1Presenter()
        {
            return new PlayerPresenter(m_gameEngineReceiver, m_eventManager, m_wonderConnector, 1);
        }

        public IPresenter CreatePlayer2Presenter()
        {
            return new PlayerPresenter(m_gameEngineReceiver, m_eventManager, m_wonderConnector, 2);
        }

        private readonly ICardConnector m_cardConnector;
        private readonly IWonderConnector m_wonderConnector;
        private readonly IGameEngineReceiver m_gameEngineReceiver;
        private readonly IEventManager m_eventManager;
        private readonly IPlayerCardHandlerFactory m_playerCardHandlerFactory;
        private readonly ISceneManager m_sceneManager;
    }
}
