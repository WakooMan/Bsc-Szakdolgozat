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
            m_gameOverScreen = m_gameEngineReceiver.ReceiveGraphicsLayer("GameOverScreen");
            m_gameResult = m_gameEngineReceiver.ReceiveTextLabel("Result");
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

            m_eventManager.Subscribe<MilitaryVictory>(eventObj =>
            {
                if (m_gameResult is not null)
                {
                    m_gameResult.Text = $"{eventObj.Player.Name} Won!";
                }
            });

            m_eventManager.Subscribe<ScientificVictory>(eventObj =>
            {
                if (m_gameResult is not null)
                {
                    m_gameResult.Text = $"{eventObj.Player.Name} Won!";
                }
            });

            m_eventManager.Subscribe<OnGameEnded>(eventObj =>
            {
                if (m_gameOverScreen is not null)
                {
                    m_gameOverScreen.Visible = true;
                }
            });
        }

        private TextLabel? m_gameResult;
        private GraphicsLayer? m_loadingScreen;
        private GraphicsLayer? m_gameOverScreen;
        private readonly IGameEngineReceiver m_gameEngineReceiver;
        private readonly IEventManager m_eventManager;
    }
}
