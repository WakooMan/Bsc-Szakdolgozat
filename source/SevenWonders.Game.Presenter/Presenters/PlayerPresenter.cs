using SevenWonders.Game.Logic.Elements;
using SevenWonders.Game.Logic.Elements.Wonders;
using SevenWonders.Game.Logic.Events;
using SevenWonders.Game.Logic.Events.GameEvents;
using SevenWonders.Game.Logic.PlayerActions;
using SevenWonders.Game.Presenter.Connectors;
using SevenWonders.Game.Presenter.Connectors.Wonders;
using SevenWonders.Game.Engine.SceneObjects;
using SevenWonders.Game.Engine.SceneHandling;

namespace SevenWonders.Game.Presenter.Presenters
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
            m_nameObject = null;
        }

        public void Initialize()
        {
            m_moneyLabel = m_gameEngineReceiver.ReceiveTextLabel($"player{m_playerId}Money");
            m_pointLabel = m_gameEngineReceiver.ReceiveTextLabel($"player{m_playerId}Points");
            m_nameLabel = m_gameEngineReceiver.ReceiveTextLabel($"player{m_playerId}Name");
            m_nameObject = m_gameEngineReceiver.ReceiveGameObject($"player{m_playerId}Name_bg");
            m_pickCardLayer = m_gameEngineReceiver.ReceiveGraphicsLayer("PickCardLayer");
            m_newTurnLayer = m_gameEngineReceiver.ReceiveGraphicsLayer("NewTurnLayer");
            foreach (var connection in m_wonderConnector.ReceiveWonderConnection())
            {
                m_wonders[connection.Key] = connection.Value;
            }
        }

        public void SubscribeToEvents()
        {
            m_eventManager.Subscribe<OnGameStarted>(OnGameStarted);
            m_eventManager.Subscribe<OnBuildWonderProcessStart>(OnBuildWonderProcessStart);
            m_eventManager.Subscribe<OnBuildWonderProcessEnd>(OnBuildWonderProcessEnd);
            m_eventManager.Subscribe<ExtraTurnGranted>(OnExtraTurnGranted);
            m_eventManager.Subscribe<TurnStarted>(OnTurnStarted);
            m_eventManager.Subscribe<ChooseWonderStarted>(OnChooseWonderStarted);
            m_eventManager.Subscribe<OnPlayerUpdate>(OnPlayerUpdate);
        }

        private void OnChooseWonderStarted(ChooseWonderStarted started)
        {
            OnPlayerTurnStart(started.Player);
        }

        private void OnTurnStarted(TurnStarted started)
        {
            OnPlayerTurnStart(started.Player);
        }

        private void OnPlayerTurnStart(Player player)
        {
            if (m_nameLabel is not null && m_nameObject is not null)
            {
                if (player.Id == m_playerId)
                {
                    m_nameObject.CurrentAnim = 1;
                    m_nameLabel.TextProperties.TextColorHex = "#FFFFFF";
                    m_nameLabel.TextProperties.Bold = true;
                }
                else
                {
                    m_nameObject.CurrentAnim = 0;
                    m_nameLabel.TextProperties.TextColorHex = "#ECECEC";
                    m_nameLabel.TextProperties.Bold = false;
                }
            }
        }

        private void OnExtraTurnGranted(ExtraTurnGranted granted)
        {
            if (m_newTurnLayer is not null)
            {
                m_newTurnLayer.Visible = true;
                Thread.Sleep(3000);
                m_newTurnLayer.Visible = false;
            }
        }

        private void OnGameStarted(OnGameStarted e)
        {
            if (e.Players.FirstOrDefault(player => player.Id == m_playerId) is Player player && e.Players.FirstOrDefault(player => player.Id != m_playerId) is Player opponent)
            {
                if(m_moneyLabel is not null && m_pointLabel is not null && m_nameLabel is not null)
                {
                    m_moneyLabel.TextProperties.Text = player.Money.ToString();
                    m_pointLabel.TextProperties.Text = player.GetPlayerProperties(opponent).VictoryPoints.ToString();
                    m_nameLabel.TextProperties.Text = player.Name;
                }
            }
        }

        private void OnPlayerUpdate(OnPlayerUpdate eventArgs)
        {
            if (eventArgs.Player1.Owner.Id == m_playerId)
            {
                UpdatePlayerState(eventArgs.Player1);
            }

            if (eventArgs.Player2.Owner.Id == m_playerId)
            {
                UpdatePlayerState(eventArgs.Player2);
            }
        }

        private void UpdatePlayerState(PlayerProperties player)
        {
            if (m_moneyLabel is not null && m_pointLabel is not null)
            {
                m_moneyLabel.TextProperties.Text = player.Owner.Money.ToString();
                m_pointLabel.TextProperties.Text = player.VictoryPoints.ToString();
                //Todo: animation for money decrease, point increase
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
        private TextLabel? m_nameLabel;
        private GameObject? m_nameObject;
        private GraphicsLayer? m_pickCardLayer;
        private GraphicsLayer? m_newTurnLayer;
    }
}
