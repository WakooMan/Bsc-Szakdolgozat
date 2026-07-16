using SevenWonders.Game.Logic.Elements;
using SevenWonders.Game.Logic.Elements.Wonders;
using SevenWonders.Game.Logic.Events;
using SevenWonders.Game.Logic.Events.GameEvents;
using SevenWonders.Game.Presenter.Connectors;
using SevenWonders.Game.Presenter.Connectors.Wonders;
using SevenWonders.Game.Presenter.GameEvents;
using System.Numerics;
using SevenWonders.Game.Engine.SceneObjects;
using SevenWonders.Game.Engine.SceneHandling;

namespace SevenWonders.Game.Presenter.Presenters
{
    public class WonderPresenter : IPresenter
    {
        public WonderPresenter(IWonderConnector wonderConnector, IGameEngineReceiver gameEngineReceiver, IEventManager eventManager)
        {
            m_wonderConnector = wonderConnector;
            m_gameEngineReceiver = gameEngineReceiver;
            m_eventManager = eventManager;
            m_wonders = new Dictionary<Wonder, WonderConnection>();
            m_player1Targets = new Stack<(GameObject, GameObject)>();
            m_player2Targets = new Stack<(GameObject, GameObject)>();
            m_centerTargets = new Stack<GameObject>();
            m_wonderDeck = null;
        }

        public void Initialize()
        {
            m_wonderDeck = m_gameEngineReceiver.ReceiveGameObject("WonderDeck");
            m_wonderLayer = m_gameEngineReceiver.ReceiveGraphicsLayer("Wonders");

            foreach (var connection in m_wonderConnector.ReceiveWonderConnection())
            {
                m_wonders.Add(connection);
            }

            List<GameObject> player1WonderTargets = m_gameEngineReceiver.ReceiveGameObjects("player1Wonder", 4).ToList();
            List<GameObject> player1CardTargets = m_gameEngineReceiver.ReceiveGameObjects("player1WonderCard", 4).ToList();

            for (int i = 0; i < 4; i++)
            {
                m_player1Targets.Push((player1WonderTargets[i], player1CardTargets[i]));
            }

            List<GameObject> player2WonderTargets = m_gameEngineReceiver.ReceiveGameObjects("player2Wonder", 4).ToList();
            List<GameObject> player2CardTargets = m_gameEngineReceiver.ReceiveGameObjects("player2WonderCard", 4).ToList();

            for (int i = 0; i < 4; i++)
            {
                m_player2Targets.Push((player2WonderTargets[i], player2CardTargets[i]));
            }

            foreach (var centerTarget in m_gameEngineReceiver.ReceiveGameObjects("centerWonder", 8))
            {
                m_centerTargets.Push(centerTarget);
            }

            m_wonderLayer.Visible = true;
        }

        public void SubscribeToEvents()
        {
            m_eventManager.Subscribe<OnGameInitialized>(eventObj =>
            {
                foreach (var connection in m_wonders)
                {
                    var group = connection.Value.GameObjectView.GetAnimationGroupBuilder().Flip("back", 0f).MoveTo(m_wonderDeck, 0f);
                    connection.Value.GameObjectView.Execute().GetAwaiter().GetResult();
                }
            });

            m_eventManager.Subscribe<OnChooseWonderStateStart>(state => {
                foreach (Wonder wonder in state.Wonders)
                {
                    MoveToCenter(wonder).GetAwaiter().GetResult();
                }
            });

            m_eventManager.Subscribe<OnFourWondersChosen>(state => {
                foreach (Wonder wonder in state.Wonders)
                {
                    MoveToCenter(wonder).GetAwaiter().GetResult();
                }
            });

            m_eventManager.Subscribe<OnWonderChosen>(state => {
                MoveToPlayer(state.Player, state.Wonder).GetAwaiter().GetResult();
            });
            m_eventManager.Subscribe<OnWonderBuilt>(eventObj =>
            {
                var connection = m_wonders[eventObj.Wonder];
                m_eventManager.Publish(new OnCardBuiltIntoWonder(eventObj, connection));
            });
        }

        private async Task MoveToPlayer(Player player, Wonder wonder)
        {
            if (player.Id == 1)
            {
                await MoveToPlayer1(wonder);
            }
            if (player.Id == 2)
            {
                await MoveToPlayer2(wonder);
            }
        }

        private async Task MoveToCenter(Wonder wonder)
        {
            if (m_centerTargets.Count > 0)
            {
                var connection = m_wonders[wonder];
                var group = connection.GameObjectView.GetAnimationGroupBuilder();
                group.MoveTo(m_centerTargets.Pop(), 1.0f)
                    .Flip("front", 1.0f);
                await connection.GameObjectView.Execute();

                group.Highlight(new Vector2(1.0f, 1.0f), true, 0.2f);
                await connection.GameObjectView.Execute();
            }
        }

        private async Task MoveToPlayer1(Wonder wonder)
        {
            if (m_player1Targets.Count > 0)
            {
                var connection = m_wonders[wonder];
                var target = m_player1Targets.Pop();
                connection.WonderTarget = target.wonderTarget;
                connection.CardTarget = target.cardTarget;


                var group = connection.GameObjectView.GetAnimationGroupBuilder();
                group.Unhighlight(false, 0.2f);
                await connection.GameObjectView.Execute();

                group.MoveTo(connection.WonderTarget, 1.0f);
                await connection.GameObjectView.Execute();
            }
        }

        private async Task MoveToPlayer2(Wonder wonder)
        {
            if (m_player2Targets.Count > 0)
            {
                var connection = m_wonders[wonder];
                var target = m_player2Targets.Pop();
                connection.WonderTarget = target.wonderTarget;
                connection.CardTarget = target.cardTarget;

                var group = connection.GameObjectView.GetAnimationGroupBuilder();
                group.Unhighlight(false, 0.2f);
                await connection.GameObjectView.Execute();

                group.MoveTo(connection.WonderTarget, 1.0f);
                await connection.GameObjectView.Execute();
            }
        }

        private readonly IDictionary<Wonder, WonderConnection> m_wonders;
        private readonly IWonderConnector m_wonderConnector;
        private readonly IGameEngineReceiver m_gameEngineReceiver;
        private readonly IEventManager m_eventManager;
        private readonly Stack<(GameObject wonderTarget, GameObject cardTarget)> m_player1Targets;
        private readonly Stack<(GameObject wonderTarget, GameObject cardTarget)> m_player2Targets;
        private readonly Stack<GameObject> m_centerTargets;
        private GameObject? m_wonderDeck;
        private GraphicsLayer? m_wonderLayer;
    }
}
