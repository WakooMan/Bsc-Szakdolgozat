using SevenWonders.Game.Logic.Elements.GameCards;
using SevenWonders.Game.Logic.Events;
using SevenWonders.Game.Logic.Handlers.Factories;
using SevenWonders.Common;

namespace SevenWonders.Game.Logic.GameStructures.Factories
{
    public class CardCompositionFactory : ICardCompositionFactory
    {
        public CardCompositionFactory(ICardCompositionFileHandlerFactory cardCompositionFileHandlerFactory, ICardNodeFactory cardNodeFactory, IEventManager eventManager)
        {
            ArgumentChecker.CheckNull(cardCompositionFileHandlerFactory, nameof(cardCompositionFileHandlerFactory));
            ArgumentChecker.CheckNull(cardNodeFactory, nameof(cardNodeFactory));
            ArgumentChecker.CheckNull(eventManager, nameof(eventManager));

            m_cardNodeFactory = cardNodeFactory;
            m_cardCompositionFileHandlerFactory = cardCompositionFileHandlerFactory;
            m_eventManager = eventManager;
        }
        public ICardComposition Create(string cardCompositionFile, ICollection<Card> cards)
        {
            ArgumentChecker.CheckNullOrEmpty(cardCompositionFile, nameof(cardCompositionFile));
            ArgumentChecker.CheckNull(cards, nameof(cards));

            return new CardComposition(m_cardCompositionFileHandlerFactory.CreateCardCompositionFileHandler(cardCompositionFile), m_cardNodeFactory, m_eventManager, cards);
        }

        private readonly ICardNodeFactory m_cardNodeFactory;
        private readonly ICardCompositionFileHandlerFactory m_cardCompositionFileHandlerFactory;
        private readonly IEventManager m_eventManager;
    }
}
