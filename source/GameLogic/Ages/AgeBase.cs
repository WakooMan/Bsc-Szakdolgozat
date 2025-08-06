using GameLogic.Elements.GameCards;
using GameLogic.GameStructures;
using GameLogic.GameStructures.Factories;
using SevenWonders.Common;

namespace GameLogic.Ages
{
    public abstract class AgeBase : IAgeBase
    {
        public abstract AgesEnum Age { get; }
        public abstract string CardCompositionFile { get; }
        public ICardComposition Composition { get; }
        public bool IsAgeOver => Composition.AvailableCards.Count <= 0;

        protected AgeBase(ICardCompositionFactory cardCompositionFactory, ICollection<Card> cards)
        {
            ArgumentChecker.CheckNull(cardCompositionFactory, nameof(cardCompositionFactory));
            ArgumentChecker.CheckNull(cards, nameof(cards));

            m_cardCompositionFactory = cardCompositionFactory;
            Composition = m_cardCompositionFactory.Create(CardCompositionFile, cards);
        }

        protected ICardCompositionFactory m_cardCompositionFactory;
    }
}
