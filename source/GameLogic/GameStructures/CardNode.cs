using GameLogic.Elements.GameCards;
using SevenWonders.Common;

namespace GameLogic.GameStructures
{
    public class CardNode : ICardNode
    {
        public Card CardObj { get; }
        public bool Hidden { get; set; }

        public IReadOnlyList<ICardNode> CoveredBy => m_coveredBy;

        public CardNode(Card cardObj)
        {
            ArgumentChecker.CheckNull(cardObj, nameof(cardObj));

            CardObj = cardObj;
            m_coveredBy = new List<ICardNode>();
        }

        public void AddParent(ICardNode cardNode)
        {
            ArgumentChecker.CheckNull(cardNode, nameof(cardNode));

            ArgumentChecker.CheckPredicateForArgument(() => cardNode == this, "Cannot add parent itself!");
            ArgumentChecker.CheckPredicateForOperation(() => m_coveredBy.Count >= 2, "Cannot add parent, because a card can only have 2 parents!");

            m_coveredBy.Add(cardNode);
        }

        public void RemoveParent(ICardNode cardNode)
        {
            m_coveredBy.Remove(cardNode);
        }

        private readonly List<ICardNode> m_coveredBy;
    }
}
