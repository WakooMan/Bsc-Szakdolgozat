using GameLogic.Elements.GameCards;
using GameLogic.GameStructures.Factories;
using GameLogic.Handlers;
using SevenWonders.Common;

namespace GameLogic.GameStructures
{
    public class CardComposition : ICardComposition
    {
        public IReadOnlyList<ICardNode> AvailableCards => m_cardNodes.Where(card => card.CoveredBy.Count <= 0).ToList();
        public IReadOnlyList<ICardNode> AllCards => m_cardNodes;

        public CardComposition(ICardCompositionFileHandler cardCompositionFileHandler, ICardNodeFactory cardNodeFactory, ICollection<Card> cards)
        {
            ArgumentChecker.CheckNull(cardCompositionFileHandler, nameof(cardCompositionFileHandler));
            ArgumentChecker.CheckNull(cardNodeFactory, nameof(cardNodeFactory));
            ArgumentChecker.CheckNull(cards, nameof(cards));
            ArgumentChecker.CheckPredicateForArgument(() => cards.Count != 20, $"Argument with name {nameof(cards)} should contain exactly 20 cards!");

            m_cardCompositionFileHandler = cardCompositionFileHandler;
            m_cardNodeFactory = cardNodeFactory;
            m_cardNodes = new List<ICardNode>();

            foreach (Card card in cards)
            {
                m_cardNodes.Add(m_cardNodeFactory.Create(card));
            }

            m_cardCompositionFileHandler.SetCompositionForCards(m_cardNodes);
        }

        public void RemoveCard(ICardNode card)
        {
            ArgumentChecker.CheckNull(card, nameof(card));

            if (AvailableCards.Contains(card))
            {
                m_cardNodes.Remove(card);
                foreach (ICardNode c in m_cardNodes)
                {
                    c.RemoveParent(card);
                    if (c.CoveredBy.Count <= 0 && c.Hidden)
                    {
                        c.Hidden = false;
                    }
                }
            }
        }

        private readonly List<ICardNode> m_cardNodes;
        private readonly ICardCompositionFileHandler m_cardCompositionFileHandler;
        private readonly ICardNodeFactory m_cardNodeFactory;
    }
}
