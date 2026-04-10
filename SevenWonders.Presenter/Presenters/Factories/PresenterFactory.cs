using GameLogic.Events;
using SevenWonders.GameEngine;
using SevenWonders.Presenter.Connectors;
using SevenWonders.Presenter.Connectors.Cards;
using SevenWonders.Presenter.Connectors.Developments;
using SevenWonders.Presenter.Connectors.MilitaryBoard;
using SevenWonders.Presenter.Connectors.Wonders;
using SevenWonders.Presenter.Views.Factories;

namespace SevenWonders.Presenter.Presenters.Factories
{
    public class PresenterFactory : IPresenterFactory
    {
        public PresenterFactory(ICardConnector cardConnector, 
                                IWonderConnector wonderConnector, 
                                IGameEngineReceiver gameEngineReceiver, 
                                IEventManager eventManager, 
                                IPlayerCardHandlerFactory playerCardHandlerFactory, 
                                ISceneManager sceneManager, 
                                IGameObjectViewFactory gameObjectViewFactory, 
                                IDevelopmentConnector developmentConnector, 
                                IObjectManager objectManager, 
                                ITextureIdHandler textureIdHandler, 
                                IMilitaryTokenChildTextureHandler militaryTokenChildTextureHandler, 
                                IDevelopmentHandlerFactory developmentHandlerFactory,
                                IGameOverHandler gameOverHandler)
        {
            m_cardConnector = cardConnector;
            m_wonderConnector = wonderConnector;
            m_gameEngineReceiver = gameEngineReceiver;
            m_eventManager = eventManager;
            m_playerCardHandlerFactory = playerCardHandlerFactory;
            m_sceneManager = sceneManager;
            m_gameObjectViewFactory = gameObjectViewFactory;
            m_developmentConnector = developmentConnector;
            m_objectManager = objectManager;
            m_textureIdHandler = textureIdHandler;
            m_militaryTokenChildTextureHandler = militaryTokenChildTextureHandler;
            m_developmentHandlerFactory = developmentHandlerFactory;
            m_gameOverHandler = gameOverHandler;
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
            return new PlayerPresenter(m_gameEngineReceiver, m_eventManager, m_wonderConnector, m_textureIdHandler, 1);
        }

        public IPresenter CreatePlayer2Presenter()
        {
            return new PlayerPresenter(m_gameEngineReceiver, m_eventManager, m_wonderConnector, m_textureIdHandler, 2);
        }

        public IPresenter CreateMilitaryBoardPresenter()
        {
            return new MilitaryBoardPresenter(m_gameEngineReceiver, m_eventManager, m_gameObjectViewFactory, m_textureIdHandler, m_militaryTokenChildTextureHandler);
        }

        public IPresenter CreateDevelopmentPresenter()
        {
            return new DevelopmentPresenter(m_developmentConnector, m_gameEngineReceiver, m_eventManager, m_developmentHandlerFactory);
        }

        public IPresenter CreateScreenPresenter()
        {
            return new ScreenPresenter(m_gameEngineReceiver, m_eventManager, m_gameOverHandler);
        }

        public IPresenter CreateChooseObjectPresenter()
        {
            return new ChooseObjectPresenter(m_gameEngineReceiver, m_eventManager, m_gameObjectViewFactory, m_objectManager);
        }

        private readonly ICardConnector m_cardConnector;
        private readonly IWonderConnector m_wonderConnector;
        private readonly IGameEngineReceiver m_gameEngineReceiver;
        private readonly IEventManager m_eventManager;
        private readonly IPlayerCardHandlerFactory m_playerCardHandlerFactory;
        private readonly ISceneManager m_sceneManager;
        private readonly IGameObjectViewFactory m_gameObjectViewFactory;
        private readonly IDevelopmentConnector m_developmentConnector;
        private readonly IObjectManager m_objectManager;
        private readonly ITextureIdHandler m_textureIdHandler;
        private readonly IMilitaryTokenChildTextureHandler m_militaryTokenChildTextureHandler;
        private readonly IDevelopmentHandlerFactory m_developmentHandlerFactory;
        private readonly IGameOverHandler m_gameOverHandler;
    }
}
