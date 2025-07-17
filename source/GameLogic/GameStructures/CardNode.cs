using GameLogic.Elements.GameCards;
using SevenWonders.Common;

namespace GameLogic.GameStructures
{
    public class CardNode : ICardNode
    {
        private List<ICardNode> coveredBy;
        public Card CardObj { get; }
        public bool Hidden { get; set; }

        public IReadOnlyList<ICardNode> CoveredBy => coveredBy;

        public CardNode(Card cardObj)
        {
            ArgumentChecker.CheckNull(cardObj, nameof(cardObj));

            CardObj = cardObj;
            coveredBy = new List<ICardNode>();
        }

        public void AddParent(ICardNode cardNode)
        {
            ArgumentChecker.CheckNull(cardNode, nameof(cardNode));

            ArgumentChecker.CheckPredicateForArgument(() => cardNode == this, "Cannot add parent itself!");
            ArgumentChecker.CheckPredicateForOperation(() => coveredBy.Count >= 2, "Cannot add parent, because a card can only have 2 parents!");

            coveredBy.Add(cardNode);
        }

        public void RemoveParent(ICardNode cardNode)
        {
            coveredBy.Remove(cardNode);
        }
    }
}
