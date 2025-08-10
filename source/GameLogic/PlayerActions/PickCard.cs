using GameLogic.Elements;
using GameLogic.Events;
using GameLogic.Events.GameEvents;
using GameLogic.GameStructures;
using SevenWonders.Common;

namespace GameLogic.PlayerActions
{
    public class PickCard : IPlayerAction
    {
        public PickCard(Player player, ICardNode cardNode)
        {
            ArgumentChecker.CheckNull(player, nameof(player));
            ArgumentChecker.CheckNull(cardNode, nameof(cardNode));

            m_player = player;
            m_cardNode = cardNode;
        }

        public bool CanPerform(IGameContext gameContext)
        {
           return  gameContext.AgeHandler.CurrentAge.Composition.AvailableCards.Contains(m_cardNode);
        }

        public void DoPlayerAction(IGameContext gameContext)
        {
            ArgumentChecker.CheckPredicateForOperation(() => !gameContext.AgeHandler.CurrentAge.Composition.AvailableCards.Contains(m_cardNode), "Action cannot be performed, because composition does not contain cardnode!");

            m_player.PickedCard = m_cardNode;
            gameContext.EventManager.Publish(new OnCardPicked(m_player, m_cardNode.CardObj));
        }

        private readonly ICardNode m_cardNode;
        private readonly Player m_player;
    }
}
