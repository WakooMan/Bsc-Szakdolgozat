using GameLogic.Elements;
using GameLogic.Events;
using GameLogic.Events.GameEvents;
using SevenWonders.GameEngine;
using SevenWonders.Presenter.Connectors;

namespace SevenWonders.Presenter.Presenters
{
    public class PlayerPresenter : IPresenter
    {
        public PlayerPresenter(IGameEngineReceiver gameEngineReceiver, IEventManager eventManager, int id)
        {
            m_gameEngineReceiver = gameEngineReceiver;
            m_eventManager = eventManager;
            m_playerId = id;
            m_moneyLabel = null;
            m_pointLabel = null;
        }

        public void Initialize()
        {
            m_moneyLabel = m_gameEngineReceiver.ReceiveTextLabel($"player{m_playerId}Money");
            m_pointLabel = m_gameEngineReceiver.ReceiveTextLabel($"player{m_playerId}Points");
        }

        public void SubscribeToEvents()
        {
            m_eventManager.Subscribe<OnGameStarted>(OnGameStarted);
            m_eventManager.Subscribe<OnCardSold>(OnCardSold);
            m_eventManager.Subscribe<OnCardBuilt>(OnCardBuilt);
        }

        private void OnGameStarted(OnGameStarted e)
        {
            if (e.Players.FirstOrDefault(player => player.Id == m_playerId) is Player player)
            {
                if(m_moneyLabel is not null && m_pointLabel is not null)
                {
                    m_moneyLabel.Text = player.Money.ToString();
                    m_pointLabel.Text = player.VictoryPoints.ToString();
                }
            }
        }

        private void OnCardSold(OnCardSold e)
        {
            if (e.Player.Id == m_playerId)
            {
                if (m_moneyLabel is not null)
                {
                    m_moneyLabel.Text = e.Player.Money.ToString();
                    //Todo: animation for money increase
                }
            }
        }

        private void OnCardBuilt(OnCardBuilt e)
        {
            if (e.Builder.Id == m_playerId)
            {
                if (m_moneyLabel is not null && m_pointLabel is not null)
                {
                    m_moneyLabel.Text = e.Builder.Money.ToString();
                    m_pointLabel.Text = e.Builder.VictoryPoints.ToString();
                    //Todo: animation for money decrease, point increase
                }
            }
        }

        private readonly IGameEngineReceiver m_gameEngineReceiver;
        private readonly IEventManager m_eventManager;
        private readonly int m_playerId;
        private TextLabel? m_moneyLabel;
        private TextLabel? m_pointLabel;
    }
}
