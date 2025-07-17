using GameLogic.Elements.GameCards;
using GameLogic.Handlers.Factories;
using SevenWonders.Common;

namespace GameLogic.GameStructures.Factories
{
    public class CardCompositionFactory : ICardCompositionFactory
    {
        public CardCompositionFactory(ICardCompositionFileHandlerFactory cardCompositionFileHandlerFactory, ICardNodeFactory cardNodeFactory)
        {
            ArgumentChecker.CheckNull(cardCompositionFileHandlerFactory, nameof(cardCompositionFileHandlerFactory));
            ArgumentChecker.CheckNull(cardNodeFactory, nameof(cardNodeFactory));

            m_cardNodeFactory = cardNodeFactory;
            m_cardCompositionFileHandlerFactory = cardCompositionFileHandlerFactory;
        }
        public ICardComposition Create(string cardCompositionFile, ICollection<Card> cards)
        {
            ArgumentChecker.CheckNullOrEmpty(cardCompositionFile, nameof(cardCompositionFile));
            ArgumentChecker.CheckNull(cards, nameof(cards));

            return new CardComposition(m_cardCompositionFileHandlerFactory.CreateCardCompositionFileHandler(cardCompositionFile), m_cardNodeFactory, cards);
        }

        private ICardNodeFactory m_cardNodeFactory;
        private ICardCompositionFileHandlerFactory m_cardCompositionFileHandlerFactory;
    }
}
