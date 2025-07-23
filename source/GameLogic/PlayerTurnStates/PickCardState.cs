using GameLogic.Elements;
using GameLogic.Events;
using GameLogic.GameStructures;
using GameLogic.Interfaces;
using GameLogic.PlayerActions;

namespace GameLogic.PlayerTurnStates
{
    public class PickCardState : IPlayerTurnState
    {
        public PickCardState(IPlayerActionReceiver playerActionReceiver , Player player, ICardComposition composition)
        {
            m_composition = composition;
            m_player = player;
            m_playerActionReceiver = playerActionReceiver;
        }

        public void ExecuteTurnState(IEventManager eventManager)
        {
            m_playerActionReceiver.ReceivePlayerAction(m_player, m_composition.AvailableCards.Select(card => (IPlayerAction)new PickCard(eventManager, m_player, card, m_composition)).ToList()).DoPlayerAction();
        }

        public IPlayerTurnState GetNextTurnState()
        {
            return new MakeActionDecision(m_playerActionReceiver, m_player, m_composition);
        }

        private readonly ICardComposition m_composition;
        private readonly Player m_player;
        private readonly IPlayerActionReceiver m_playerActionReceiver;
    }
}
