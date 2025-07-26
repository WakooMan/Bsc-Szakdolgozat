using GameLogic.Elements;
using GameLogic.Events;
using GameLogic.GameStructures;
using GameLogic.Handlers;
using GameLogic.Interfaces;
using GameLogic.PlayerActions;

namespace GameLogic.PlayerTurnStates
{
    public class MakeActionDecision : IPlayerTurnState
    {
        public bool GoToPrevState { get; set; }

        public MakeActionDecision(IEventManager eventManager, ICostCalculator costCalculator, IPlayerActionReceiver playerActionReceiver, ICardComposition composition, Player player, Player opponent)
        {
            m_eventManager = eventManager;
            m_costCalculator = costCalculator;
            m_playerActionReceiver = playerActionReceiver;
            m_composition = composition;
            m_player = player;
            m_opponent = opponent;
            GoToPrevState = false;
        }

        public void ExecuteTurnState(IEventManager eventManager)
        {
            List<IPlayerAction> playerActions =
            [
                new UnpickCard(eventManager, this, m_player), new BuildCard(m_eventManager, m_costCalculator, m_composition, m_player, m_opponent), new SellCard(m_composition, m_player),
                .. m_player.Wonders.Select(wonder => new BuildWonder(m_eventManager, m_costCalculator, m_composition, wonder, m_player, m_opponent)),
            ];

            m_playerActionReceiver.ReceivePlayerAction(m_player, playerActions).DoPlayerAction();
        }

        public IPlayerTurnState GetNextTurnState()
        {
            return GoToPrevState ? new PickCardState(m_playerActionReceiver, m_composition, m_eventManager, m_costCalculator, m_player, m_opponent) : new EndTurn();
        }

        private readonly Player m_player;
        private readonly Player m_opponent;
        private readonly ICardComposition m_composition;
        private readonly IPlayerActionReceiver m_playerActionReceiver;
        private readonly ICostCalculator m_costCalculator;
        private readonly IEventManager m_eventManager;
    }
}
