using GameLogic.Elements.GameCards;
using GameLogic.Events;
using GameLogic.GameStructures.Factories;
using GameLogic.Handlers;

namespace GameLogic.Ages
{
    public class SecondAge : AgeBase
    {
        public override AgesEnum Age => AgesEnum.II;

        public override string CardCompositionFile => "GameLogic.Data.SecondAgeComposition.csv";

        public SecondAge(IEventManager eventManager, ICardCompositionFactory cardCompositionFactory, ICardList cardList, IRandomElementReceiver randomElementReceiver) : base(eventManager, cardCompositionFactory, randomElementReceiver.ReceiveRandomElements(cardList?.Cards.Where(card => card.Age == AgesEnum.II).ToArray() ?? [], 20))
        { }
    }
}
