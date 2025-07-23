using GameLogic.Elements;
using GameLogic.Events;
using GameLogic.GameStructures;

namespace GameLogic.PlayerActions
{
    public class PickCard : IPlayerAction
    {
        public PickCard(IEventManager eventManager, Player player, ICardNode cardNode, ICardComposition cardComposition)
        {
            m_eventManager = eventManager;
            m_player = player;
            m_cardNode = cardNode;
            m_cardComposition = cardComposition;
        }

        public bool CanPerform()
        {
           return  m_cardComposition.AvailableCards.Contains(m_cardNode);
        }

        public void DoPlayerAction()
        {
            m_player.PickedCard = m_cardNode;
            m_eventManager.Publish(GameEventType.CardPicked, new OnCardPicked(m_cardNode.CardObj));
        }

        private readonly ICardNode m_cardNode;
        private readonly ICardComposition m_cardComposition;
        private readonly Player m_player;
        private readonly IEventManager m_eventManager;
    }
}
