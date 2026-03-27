using GameLogic.Elements;
using GameLogic.Elements.Wonders;
using GameLogic.Events;
using GameLogic.Events.GameEvents;
using SevenWonders.GameEngine;
using SevenWonders.Presenter.Connectors;
using SevenWonders.Presenter.Views;
using System.Numerics;

namespace SevenWonders.Presenter.Presenters
{
    public class WonderPresenter : IWonderPresenter
    {
        public WonderPresenter(IWonderConnector wonderConnector, IGameEngineReceiver gameEngineReceiver, IEventManager eventManager)
        {
            m_wonderConnector = wonderConnector;
            m_gameEngineReceiver = gameEngineReceiver;
            m_eventManager = eventManager;
            m_wonders = new Dictionary<Wonder, IGameObjectView>();
            m_player1Targets = new Stack<GameObject>();
            m_player2Targets = new Stack<GameObject>();
            m_centerTargets = new Stack<GameObject>();
            m_wonderDeck = null;
        }

        public void Initialize()
        {
            m_wonderDeck = m_gameEngineReceiver.ReceiveGameObject("WonderDeck");
            m_wonderLayer = m_gameEngineReceiver.ReceiveGraphicsLayer("Wonders");

            foreach (var connection in m_wonderConnector.ReceiveWonderConnection())
            {
                m_wonders[connection.Key] = connection.Value;
                var group = connection.Value.GetAnimationGroupBuilder().Flip(1, 0f).MoveTo(m_wonderDeck, 0f);
                connection.Value.Execute();
            }

            foreach (var player1Target in m_gameEngineReceiver.ReceiveGameObjects("player1Wonder", 4))
            {
                m_player1Targets.Push(player1Target);
            }

            foreach (var player2Target in m_gameEngineReceiver.ReceiveGameObjects("player2Wonder", 4))
            {
                m_player2Targets.Push(player2Target);
            }

            foreach (var centerTarget in m_gameEngineReceiver.ReceiveGameObjects("centerWonder", 8))
            {
                m_centerTargets.Push(centerTarget);
            }

            m_wonderLayer.Visible = true;
        }

        public void SubscribeToEvents()
        {
            m_eventManager.Subscribe<OnChooseWonderStateStart>(state => {
                foreach (Wonder wonder in state.Wonders)
                {
                    MoveToCenter(wonder);
                }
            });

            m_eventManager.Subscribe<OnFourWondersChosen>(state => {
                foreach (Wonder wonder in state.Wonders)
                {
                    MoveToCenter(wonder);
                }
            });

            m_eventManager.Subscribe<OnChooseWonderStateEnd>(state => {
                foreach (Wonder wonder in state.Wonders)
                {
                    MoveToDeck(wonder);
                }
            });

            m_eventManager.Subscribe<OnWonderChosen>(state => {
                MoveToPlayer(state.Player, state.Wonder);
            });
        }

        private void MoveToPlayer(Player player, Wonder wonder)
        {
            if (player.Id == 1)
            {
                MoveToPlayer1(wonder);
            }
            if (player.Id == 2)
            {
                MoveToPlayer2(wonder);
            }
        }

        private void MoveToCenter(Wonder wonder)
        {
            if (m_centerTargets.Count > 0)
            {
                var view = m_wonders[wonder];
                var group = view.GetAnimationGroupBuilder();
                group.MoveTo(m_centerTargets.Pop(), 1.0f)
                .Flip(0, 1.0f);
                view.Execute();

                group.Highlight(new Vector2(1.0f, 1.0f), true, 0.2f);
                view.Execute();
            }
        }

        private void MoveToDeck(Wonder wonder)
        {
            if (m_wonderDeck is not null)
            {
                var view = m_wonders[wonder];
                view.UnsubscribeClick();

                var group = view.GetAnimationGroupBuilder();
                group.Unhighlight(false, 0.2f);
                view.Execute();

                group.MoveTo(m_wonderDeck, 1.0f)
                .Flip(1, 1.0f);
                view.Execute();
            }
        }

        private void MoveToPlayer1(Wonder wonder)
        {
            if (m_player1Targets.Count > 0)
            {
                var view = m_wonders[wonder];
                view.UnsubscribeClick();

                var group = view.GetAnimationGroupBuilder();
                group.Unhighlight(false, 0.2f);
                view.Execute();

                group.MoveTo(m_player1Targets.Pop(), 1.0f);
                view.Execute();
            }
        }

        private void MoveToPlayer2(Wonder wonder)
        {
            if (m_player2Targets.Count > 0)
            {
                var view = m_wonders[wonder];
                view.UnsubscribeClick();

                var group = view.GetAnimationGroupBuilder();
                group.Unhighlight(false, 0.2f);
                view.Execute();

                group.MoveTo(m_player2Targets.Pop(), 1.0f);
                view.Execute();
            }
        }

        private readonly IDictionary<Wonder, IGameObjectView> m_wonders;
        private readonly IWonderConnector m_wonderConnector;
        private readonly IGameEngineReceiver m_gameEngineReceiver;
        private readonly IEventManager m_eventManager;
        private readonly Stack<GameObject> m_player1Targets;
        private readonly Stack<GameObject> m_player2Targets;
        private readonly Stack<GameObject> m_centerTargets;
        private GameObject? m_wonderDeck;
        private GraphicsLayer? m_wonderLayer;
    }
}
