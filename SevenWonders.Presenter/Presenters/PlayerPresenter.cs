using GameLogic.Elements;
using GameLogic.Elements.Wonders;
using GameLogic.Events;
using GameLogic.Events.GameEvents;
using GameLogic.PlayerActions;
using Microsoft.Maui.Controls;
using SevenWonders.GameEngine;
using SevenWonders.Presenter.Connectors;
using SevenWonders.Presenter.Connectors.Wonders;
using SevenWonders.Presenter.Views;
using SevenWonders.Presenter.Views.Factories;

namespace SevenWonders.Presenter.Presenters
{
    public class PlayerPresenter : IPresenter
    {
        public PlayerPresenter(IGameEngineReceiver gameEngineReceiver, IEventManager eventManager, IWonderConnector wonderConnector, int id)
        {
            m_gameEngineReceiver = gameEngineReceiver;
            m_eventManager = eventManager;
            m_wonderConnector = wonderConnector;
            m_wonders = new Dictionary<Wonder, WonderConnection>();
            m_playerId = id;
            m_moneyLabel = null;
            m_pointLabel = null;
        }

        public void Initialize()
        {
            m_moneyLabel = m_gameEngineReceiver.ReceiveTextLabel($"player{m_playerId}Money");
            m_pointLabel = m_gameEngineReceiver.ReceiveTextLabel($"player{m_playerId}Points");
            m_pickCardLayer = m_gameEngineReceiver.ReceiveGraphicsLayer("PickCardLayer");
            foreach (var connection in m_wonderConnector.ReceiveWonderConnection())
            {
                m_wonders[connection.Key] = connection.Value;
            }
        }

        public void SubscribeToEvents()
        {
            m_eventManager.Subscribe<OnGameStarted>(OnGameStarted);
            m_eventManager.Subscribe<OnCardSold>(OnCardSold);
            m_eventManager.Subscribe<OnCardBuilt>(OnCardBuilt);
            m_eventManager.Subscribe<OnBuildWonderProcessStart>(OnBuildWonderProcessStart);
            m_eventManager.Subscribe<OnBuildWonderProcessEnd>(OnBuildWonderProcessEnd);
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

        private void OnBuildWonderProcessStart(OnBuildWonderProcessStart e)
        {
            var button = m_gameEngineReceiver.ReceiveButton(e.BackAction.Name);
            if (m_pickCardLayer is not null)
            {
                m_pickCardLayer.Visible = false;
            }
            button.Visible = true;

            foreach (BuildWonder action in e.BuildWonderActions)
            {
                WonderConnection connection = m_wonders[action.Wonder];
                if (!connection.GameObjectView.IsDimmed)
                {
                    var group = connection.GameObjectView.GetAnimationGroupBuilder();
                    group.Unhighlight(true, 0f);
                    connection.GameObjectView.Execute().GetAwaiter().GetResult();
                }
                
            }
        }

        private void OnBuildWonderProcessEnd(OnBuildWonderProcessEnd e)
        {
            var button = m_gameEngineReceiver.ReceiveButton(e.BackAction.Name);
            button.Visible = false;

            if (m_pickCardLayer is not null)
            {
                m_pickCardLayer.Visible = !e.IsCompleted;
            }

            foreach (BuildWonder action in e.BuildWonderActions)
            {
                WonderConnection connection = m_wonders[action.Wonder];
                var group = connection.GameObjectView.GetAnimationGroupBuilder();
                group.Unhighlight(false, 0f);
                connection.GameObjectView.Execute().GetAwaiter().GetResult();
            }
        }

        private readonly IGameEngineReceiver m_gameEngineReceiver;
        private readonly IEventManager m_eventManager;
        private readonly IWonderConnector m_wonderConnector;
        private readonly Dictionary<Wonder, WonderConnection> m_wonders;
        private readonly int m_playerId;
        private TextLabel? m_moneyLabel;
        private TextLabel? m_pointLabel;
        private GraphicsLayer? m_pickCardLayer;
    }
}
