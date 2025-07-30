using GameLogic.Elements;
using GameLogic.Elements.GameCards;
using GameLogic.Events;

namespace GameLogic.PlayerActions
{
    public class DropCard : IPlayerAction
    {
        public DropCard(IEventManager eventManager, Player player, Card card)
        {
            m_eventManager = eventManager;
            m_player = player;
            m_card = card;
        }
        public bool CanPerform()
        {
            return m_player.Cards.Contains(m_card);
        }

        public void DoPlayerAction()
        {
            m_player.Cards.Remove(m_card);
            m_eventManager.Publish(GameEventType.CardDestroyed, new OnCardDestroyed(m_player, m_card));
        }

        private readonly Card m_card;
        private readonly Player m_player;
        private readonly IEventManager m_eventManager;
    }
}
