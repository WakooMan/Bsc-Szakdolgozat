using GameLogic.Elements;
using GameLogic.Events;
using GameLogic.GameStructures;
using GameLogic.Handlers;
using GameLogic.Interfaces;
using GameLogic.PlayerActions;

namespace GameLogic.PlayerTurnStates
{
    public class PickCardState : IPlayerTurnState
    {
        public PickCardState(IPlayerActionReceiver playerActionReceiver,ICardComposition composition, IEventManager eventManager,ICostCalculator costCalculator, Player player, Player opponent)
        {
            m_composition = composition;
            m_player = player;
            m_playerActionReceiver = playerActionReceiver;
            m_eventManager = eventManager;
            m_costCalculator = costCalculator;
            m_opponent = opponent;
        }

        public void ExecuteTurnState(IEventManager eventManager)
        {
            m_playerActionReceiver.ReceivePlayerAction(m_player, m_composition.AvailableCards.Select(card => (IPlayerAction)new PickCard(eventManager, m_player, card, m_composition)).ToList()).DoPlayerAction();
        }

        public IPlayerTurnState GetNextTurnState()
        {
            return new MakeActionDecision(m_eventManager, m_costCalculator, m_playerActionReceiver, m_composition, m_player, m_opponent);
        }

        private readonly ICardComposition m_composition;
        private readonly Player m_player;
        private readonly Player m_opponent;
        private readonly IPlayerActionReceiver m_playerActionReceiver;
        private readonly IEventManager m_eventManager;
        private readonly ICostCalculator m_costCalculator;
    }
}
