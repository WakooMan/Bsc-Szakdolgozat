using GameLogic.Elements.Wonders;
using SevenWonders.GameEngine;
using SevenWonders.Presenter.Connectors;
using SevenWonders.Presenter.Views;

namespace SevenWonders.Presenter.Presenters
{
    public class WonderPresenter : IWonderPresenter
    {
        public WonderPresenter(IWonderConnector wonderConnector)
        {
            m_wonderConnector = wonderConnector;
            m_wonders = new Dictionary<Wonder, IWonderView>();
            m_player1Targets = new Stack<GameObject>();
            m_player2Targets = new Stack<GameObject>();
            m_centerTargets = new Stack<GameObject>();
        }

        public void Initialize()
        {
            foreach (var connection in m_wonderConnector.CreateWonderConnection())
            {
                m_wonders[connection.Key] = connection.Value;
            }

            foreach (var player1Target in m_wonderConnector.CreatePlayer1TargetList())
            {
                m_player1Targets.Push(player1Target);
            }

            foreach (var player2Target in m_wonderConnector.CreatePlayer2TargetList())
            {
                m_player2Targets.Push(player2Target);
            }

            foreach (var centerTarget in m_wonderConnector.CreateCenterTargetList())
            {
                m_centerTargets.Push(centerTarget);
            }
        }

        public void MoveToPlayer1(Wonder wonder)
        {
            if (m_player1Targets.Count > 0)
            {
                m_wonders[wonder].MoveTo(m_player1Targets.Pop());
            }
        }

        public void MoveToPlayer2(Wonder wonder)
        {
            if (m_player2Targets.Count > 0)
            {
                m_wonders[wonder].MoveTo(m_player2Targets.Pop());
            }
        }

        public void MoveToCenter(Wonder wonder)
        {
            if (m_centerTargets.Count > 0)
            {
                m_wonders[wonder].MoveTo(m_centerTargets.Pop());
            }
        }

        private readonly IDictionary<Wonder, IWonderView> m_wonders;
        private readonly IWonderConnector m_wonderConnector;
        private readonly Stack<GameObject> m_player1Targets;
        private readonly Stack<GameObject> m_player2Targets;
        private readonly Stack<GameObject> m_centerTargets;
    }
}
