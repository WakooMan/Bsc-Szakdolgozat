using GameLogic.Elements.GameCards;
using GameLogic.Handlers;

namespace GameLogic.GameStructures
{
    public class CardComposition
    {
        private List<CardNode> m_cardNodes;
        private ICardCompositionFileHandler m_cardCompositionFileHandler;
        public IReadOnlyList<CardNode> AvailableCards => m_cardNodes.Where(card => card.CoveredBy.Count <= 0).ToList();

        public CardComposition(ICardCompositionFileHandler cardCompositionFileHandler, List<Card> cards)
        {
            if (cardCompositionFileHandler is null)
            {
                throw new ArgumentNullException($"Argument with name {nameof(cardCompositionFileHandler)} is null!");
            }

            if (cards.Count != 20)
            {
                throw new ArgumentException($"Argument with name {nameof(cards)} should contain exactly 20 cards!");
            }

            m_cardCompositionFileHandler = cardCompositionFileHandler;
            m_cardNodes = new List<CardNode>();

            foreach (Card card in cards)
            {
                m_cardNodes.Add(new CardNode(card));
            }

            m_cardCompositionFileHandler.SetCompositionForCards(m_cardNodes);
        }

        public void RemoveCard(CardNode card)
        {
            if (AvailableCards.Contains(card))
            {
                m_cardNodes.Remove(card);
                foreach (CardNode c in m_cardNodes)
                {
                    c.RemoveParent(card);
                    if (c.CoveredBy.Count <= 0 && c.Hidden)
                    {
                        c.Hidden = false;
                    }
                }
            }
        }
    }
}
