using GameLogic.Elements;
using GameLogic.Elements.GameCards;
using GameLogic.Events;

namespace GameLogic.PlayerActions
{
    public class UnpickCard : IPlayerAction
    {
        public UnpickCard(IEventManager eventManager, Player player)
        {
            m_eventManager = eventManager;
            m_player = player;
        }

        public bool CanPerform()
        {
            return m_player.PickedCard is not null;
        }

        public void DoPlayerAction()
        {
            Card card = m_player.PickedCard.CardObj;
            m_player.PickedCard = null;
            m_eventManager.Publish(GameEventType.CardUnpicked, new OnCardUnpicked(m_player, card));
        }

        private readonly Player m_player;
        private readonly IEventManager m_eventManager;
    }
}
