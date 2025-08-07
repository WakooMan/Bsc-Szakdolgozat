using GameLogic.Elements;
using GameLogic.Events;
using GameLogic.Events.GameEvents;
using GameLogic.GameStructures;

namespace GameLogic.PlayerActions
{
    public class PickCard : IPlayerAction
    {
        public PickCard(Player player, ICardNode cardNode)
        {
            m_player = player;
            m_cardNode = cardNode;
        }

        public bool CanPerform(IGameContext gameContext)
        {
           return  gameContext.AgeHandler.CurrentAge.Composition.AvailableCards.Contains(m_cardNode);
        }

        public void DoPlayerAction(IGameContext gameContext)
        {
            m_player.PickedCard = m_cardNode;
            gameContext.EventManager.Publish(new OnCardPicked(m_player, m_cardNode.CardObj));
        }

        private readonly ICardNode m_cardNode;
        private readonly Player m_player;
    }
}
