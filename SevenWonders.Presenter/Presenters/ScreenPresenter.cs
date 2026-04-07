using GameLogic.Events;
using GameLogic.Events.GameEvents;
using SevenWonders.GameEngine;
using SevenWonders.Presenter.Connectors;

namespace SevenWonders.Presenter.Presenters
{
    public class ScreenPresenter : IPresenter
    {
        public ScreenPresenter(IGameEngineReceiver gameEngineReceiver, IEventManager eventManager)
        {
            m_gameEngineReceiver = gameEngineReceiver;
            m_eventManager = eventManager;
        }

        public void Initialize()
        {
            m_loadingScreen = m_gameEngineReceiver.ReceiveGraphicsLayer("LoadingScreen");
        }

        public void SubscribeToEvents()
        {
            m_eventManager.Subscribe<OnGameStarted>(eventObj =>
            {
                if (m_loadingScreen is not null)
                {
                    m_loadingScreen.Visible = false;
                }
            });
        }

        private GraphicsLayer? m_loadingScreen;
        private readonly IGameEngineReceiver m_gameEngineReceiver;
        private readonly IEventManager m_eventManager;
    }
}
