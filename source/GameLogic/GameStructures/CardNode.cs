using GameLogic.Elements.GameCards;

namespace GameLogic.GameStructures
{
    public class CardNode
    {
        private List<CardNode> coveredBy;
        public Card CardObj { get; }
        public bool Hidden { get; set; }

        public IReadOnlyList<CardNode> CoveredBy => coveredBy;

        public CardNode(Card cardObj)
        {
            CardObj = cardObj;
            coveredBy = new List<CardNode>();
        }

        public void AddParent(CardNode cardNode)
        {
            if (coveredBy.Count >= 2)
            {
                throw new InvalidOperationException("Cannot add parent, because a card can only have 2 parents!");
            }

            coveredBy.Add(cardNode);
        }

        public void RemoveParent(CardNode cardNode)
        {
            coveredBy.Remove(cardNode);
        }
    }
}
