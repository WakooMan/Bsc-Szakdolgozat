using GameLogic.Elements;
using GameLogic.Events;
using GameLogic.Interfaces;
using SevenWonders.Common;

namespace GameLogic.Handlers
{
    public class TurnHandler: ITurnHandler
    {
        public Player CurrentPlayer => m_players[m_index];
        public Player OpponentPlayer => m_players[(m_index + 1 < m_players.Count) ? m_index + 1 : 0];

        public void NextPlayer()
        {
            m_eventManager.Publish(GameEventType.TurnEnded, new TurnEnded(this));
            if (!m_newTurnForced)
            {
                m_index = (m_index + 1 < m_players.Count) ? m_index + 1 : 0;
            }
            m_eventManager.Publish(GameEventType.TurnStarted, new TurnStarted(m_playerActionReceiver, this));
            m_newTurnForced = false;
        }

        public void ForceNewTurn()
        {
            m_newTurnForced = true;
        }

        public TurnHandler(IPlayerActionReceiver playerActionReceiver, IEventManager eventManager, ICollection<Player> players)
        {
            ArgumentChecker.CheckNull(playerActionReceiver, nameof(playerActionReceiver));
            ArgumentChecker.CheckNull(eventManager, nameof(eventManager));
            ArgumentChecker.CheckNull(players, nameof(players));
            ArgumentChecker.CheckPredicateForArgument(() => players.Count != 2, "Number of players must be exactly 2!");

            m_playerActionReceiver = playerActionReceiver;
            m_eventManager = eventManager;
            m_players = new List<Player>(players);
            m_index = 0;
            m_newTurnForced = false;
        }

        private readonly List<Player> m_players;
        private int m_index;
        private bool m_newTurnForced;
        private readonly IEventManager m_eventManager;
        private readonly IPlayerActionReceiver m_playerActionReceiver;
    }
}
