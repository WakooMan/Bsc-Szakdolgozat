using SevenWonders.Game.Logic.Elements;
using SevenWonders.Game.Logic.Events.GameEvents;
using SevenWonders.Game.Logic.GameStructures;
using SevenWonders.Common;

namespace SevenWonders.Game.Logic.PlayerActions
{
    public class PickCard : IPlayerAction
    {
        public string Name => m_cardNode.CardObj.Name;
        public int Id => 11;
        public ICardNode CardNode => m_cardNode;
        public Player Player => m_player;
        public PickCard() { }
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

        public bool DoPlayerAction(IGameContext gameContext)
        {
            ArgumentChecker.CheckPredicateForOperation(() => !gameContext.AgeHandler.CurrentAge.Composition.AvailableCards.Contains(m_cardNode), "Action cannot be performed, because composition does not contain cardnode!");

            m_player.PickedCard = m_cardNode;
            gameContext.EventManager.Publish(new OnCardPicked(m_player, m_cardNode.CardObj));
            return true;
        }

        private readonly ICardNode m_cardNode;
        private readonly Player m_player;
    }
}
