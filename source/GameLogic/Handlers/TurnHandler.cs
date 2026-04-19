using GameLogic.Elements;
using GameLogic.Events;
using GameLogic.Events.GameEvents;
using SevenWonders.Common;

namespace GameLogic.Handlers
{
    public class TurnHandler: ITurnHandler
    {
        public Player CurrentPlayer
        {
            get
            {
                if (m_players is null)
                {
                    throw new InvalidOperationException("Cannot get CurrentPlayer until SetPlayers method is not called!");
                }
                return m_players[m_index];
            }
        }
        public Player OpponentPlayer
        {
            get
            {
                if (m_players is null)
                {
                    throw new InvalidOperationException("Cannot get OpponentPlayer until SetPlayers method is not called!");
                }
                return m_players[(m_index + 1 < m_players.Count) ? m_index + 1 : 0];
            }
        }

        public Player GetPlayer(int id)
        {
            if (m_players is null)
            {
                throw new InvalidOperationException("Cannot get players until SetPlayers method is not called!");
            }

            foreach (Player player in m_players)
            {
                if (player.Id == id)
                {
                    return player;
                }    
            }

            throw new ArgumentException($"Player with id {id} not found!");
        }

        public TurnHandler(IEventManager eventManager)
        {
            ArgumentChecker.CheckNull(eventManager, nameof(eventManager));

            m_eventManager = eventManager;
            m_players = null;
        }

        public void Initialize(ICollection<Player> players)
        {
            ArgumentChecker.CheckNull(players, nameof(players));
            ArgumentChecker.CheckPredicateForArgument(() => players.Count != 2, "Number of players must be exactly 2!");

            m_players = [.. players];
            m_index = 0;
            m_newTurnForced = false;
        }

        public void NextPlayer()
        {
            if (m_players is null)
            {
                throw new InvalidOperationException("Cannot execute NextPlayer method until SetPlayers method is not called!");
            }

            GameLog.Info($"NextPlayer called. Current={CurrentPlayer.Name}, NewTurnForced={m_newTurnForced}");
            m_eventManager.Publish(new TurnEnded(CurrentPlayer));
            if (!m_newTurnForced)
            {
                m_index = (m_index + 1 < m_players.Count) ? m_index + 1 : 0;
            }
            m_newTurnForced = false;
        }

        public void ForceNewTurn()
        {
            if (m_players is null)
            {
                throw new InvalidOperationException("Cannot execute ForceNewTurn method until SetPlayers method is not called!");
            }

            GameLog.Info($"ForceNewTurn called for player: {CurrentPlayer.Name}");
            m_newTurnForced = true;
            m_eventManager.Publish(new ExtraTurnGranted());
        }

        private List<Player>? m_players;
        private int m_index;
        private bool m_newTurnForced;
        private readonly IEventManager m_eventManager;
    }
}
