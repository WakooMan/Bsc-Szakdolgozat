using GameLogic.Elements;
using GameLogic.Elements.Wonders;
using SevenWonders.GameEngine;
using SevenWonders.Presenter.Connectors;
using SevenWonders.Presenter.Views;
using static SevenWonders.Presenter.Presenters.IWonderPresenter;

namespace SevenWonders.Presenter.Presenters
{
    public class WonderPresenter : IWonderPresenter
    {
        public event WonderPresenterDelegate? WonderChosen;

        public WonderPresenter(IWonderConnector wonderConnector, IGameObjectReceiver gameObjectReceiver)
        {
            m_wonderConnector = wonderConnector;
            m_gameObjectReceiver = gameObjectReceiver;
            m_wonders = new Dictionary<Wonder, IGameObjectView>();
            m_player1Targets = new Stack<GameObject>();
            m_player2Targets = new Stack<GameObject>();
            m_centerTargets = new Stack<GameObject>();
            m_wonderDeck = null;
        }

        public void Initialize()
        {
            m_wonderDeck = m_gameObjectReceiver.ReceiveGameObject("WonderDeck");

            foreach (var connection in m_wonderConnector.ReceiveWonderConnection())
            {
                m_wonders[connection.Key] = connection.Value;
            }

            foreach (var player1Target in m_gameObjectReceiver.ReceiveGameObjects("player1Wonder", 4))
            {
                m_player1Targets.Push(player1Target);
            }

            foreach (var player2Target in m_gameObjectReceiver.ReceiveGameObjects("player2Wonder", 4))
            {
                m_player2Targets.Push(player2Target);
            }

            foreach (var centerTarget in m_gameObjectReceiver.ReceiveGameObjects("centerWonder", 8))
            {
                m_centerTargets.Push(centerTarget);
            }
        }

        public void MoveToPlayer(Player player, Wonder wonder)
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

        public void MoveToCenter(Wonder wonder)
        {
            if (m_centerTargets.Count > 0)
            {
                m_wonders[wonder].MoveTo(m_centerTargets.Pop());
                m_wonders[wonder].Highlight();
                m_wonders[wonder].SubscribeClickAtAnimationEnd(() => WonderChosen?.Invoke(wonder));
            }
        }

        public void MoveToDeck(Wonder wonder)
        {
            if (m_wonderDeck is not null)
            {
                m_wonders[wonder].UnsubscribeClick();
                m_wonders[wonder].Unhighlight();
                m_wonders[wonder].MoveTo(m_wonderDeck);
            }
        }

        private void MoveToPlayer1(Wonder wonder)
        {
            if (m_player1Targets.Count > 0)
            {
                m_wonders[wonder].UnsubscribeClick();
                m_wonders[wonder].Unhighlight();
                m_wonders[wonder].MoveTo(m_player1Targets.Pop());
            }
        }

        private void MoveToPlayer2(Wonder wonder)
        {
            if (m_player2Targets.Count > 0)
            {
                m_wonders[wonder].UnsubscribeClick();
                m_wonders[wonder].Unhighlight();
                m_wonders[wonder].MoveTo(m_player2Targets.Pop());
            }
        }

        private readonly IDictionary<Wonder, IGameObjectView> m_wonders;
        private readonly IWonderConnector m_wonderConnector;
        private readonly IGameObjectReceiver m_gameObjectReceiver;
        private readonly Stack<GameObject> m_player1Targets;
        private readonly Stack<GameObject> m_player2Targets;
        private readonly Stack<GameObject> m_centerTargets;
        private GameObject? m_wonderDeck;
    }
}
