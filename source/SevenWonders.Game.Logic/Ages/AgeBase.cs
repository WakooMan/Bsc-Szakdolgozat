using SevenWonders.Game.Logic.Elements.GameCards;
using SevenWonders.Game.Logic.Events;
using SevenWonders.Game.Logic.Events.GameEvents;
using SevenWonders.Game.Logic.GameStructures;
using SevenWonders.Game.Logic.GameStructures.Factories;
using SevenWonders.Common;

namespace SevenWonders.Game.Logic.Ages
{
    public abstract class AgeBase : IAgeBase
    {
        public abstract AgesEnum Age { get; }
        public abstract string CardCompositionFile { get; }
        public ICardComposition Composition { get; }
        public bool IsAgeOver => Composition.AvailableCards.Count <= 0;

        protected AgeBase(IEventManager eventManager, ICardCompositionFactory cardCompositionFactory, ICollection<Card>? cards)
        {
            ArgumentChecker.CheckNull(eventManager, nameof(eventManager));
            ArgumentChecker.CheckNull(cardCompositionFactory, nameof(cardCompositionFactory));
            ArgumentChecker.CheckNull(cards, nameof(cards));

            m_cardCompositionFactory = cardCompositionFactory;
            Composition = m_cardCompositionFactory.Create(CardCompositionFile, cards);
        }

        protected ICardCompositionFactory m_cardCompositionFactory;
    }
}
