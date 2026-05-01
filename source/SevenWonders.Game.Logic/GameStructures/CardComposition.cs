using SevenWonders.Game.Logic.Elements.GameCards;
using SevenWonders.Game.Logic.Events;
using SevenWonders.Game.Logic.Events.GameEvents;
using SevenWonders.Game.Logic.GameStructures.Factories;
using SevenWonders.Game.Logic.Handlers;
using SevenWonders.Common;

namespace SevenWonders.Game.Logic.GameStructures
{
    public class CardComposition : ICardComposition
    {
        public IReadOnlyList<ICardNode> AvailableCards => m_cardNodes.Where(card => card.CoveredBy.Count <= 0).ToList();
        public IReadOnlyList<ICardNode> AllCards => m_cardNodes;

        public CardComposition(ICardCompositionFileHandler cardCompositionFileHandler, ICardNodeFactory cardNodeFactory, IEventManager eventManager, ICollection<Card> cards)
        {
            ArgumentChecker.CheckNull(cardCompositionFileHandler, nameof(cardCompositionFileHandler));
            ArgumentChecker.CheckNull(cardNodeFactory, nameof(cardNodeFactory));
            ArgumentChecker.CheckNull(eventManager, nameof(eventManager));
            ArgumentChecker.CheckNull(cards, nameof(cards));
            ArgumentChecker.CheckPredicateForArgument(() => cards.Count != 20, $"Argument with name {nameof(cards)} should contain exactly 20 cards!");

            m_cardCompositionFileHandler = cardCompositionFileHandler;
            m_cardNodeFactory = cardNodeFactory;
            m_eventManager = eventManager;
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
                        m_eventManager.Publish(new CardNodeAvailableEvent(c));
                    }
                }
            }
        }

        private readonly List<ICardNode> m_cardNodes;
        private readonly ICardCompositionFileHandler m_cardCompositionFileHandler;
        private readonly ICardNodeFactory m_cardNodeFactory;
        private readonly IEventManager m_eventManager;
    }
}
