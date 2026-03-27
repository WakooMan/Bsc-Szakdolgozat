using GameLogic.Events;
using SevenWonders.Presenter.Connectors;
using SevenWonders.Presenter.Connectors.Cards;

namespace SevenWonders.Presenter.Presenters.Factories
{
    public class PresenterFactory : IPresenterFactory
    {
        public PresenterFactory(ICardConnector cardConnector, IWonderConnector wonderConnector, IGameEngineReceiver gameEngineReceiver, IEventManager eventManager)
        {
            m_cardConnector = cardConnector;
            m_wonderConnector = wonderConnector;
            m_gameEngineReceiver = gameEngineReceiver;
            m_eventManager = eventManager;
        }

        public IPresenter CreateCardPresenter()
        {
            return new CardPresenter(m_cardConnector, m_gameEngineReceiver, m_eventManager);
        }

        public IPresenter CreateWonderPresenter()
        {
            return new WonderPresenter(m_wonderConnector, m_gameEngineReceiver, m_eventManager);
        }

        private readonly ICardConnector m_cardConnector;
        private readonly IWonderConnector m_wonderConnector;
        private readonly IGameEngineReceiver m_gameEngineReceiver;
        private readonly IEventManager m_eventManager;
    }
}
