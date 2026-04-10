using GameLogic.Elements.GameCards;
using GameLogic.Events;
using GameLogic.GameStructures.Factories;
using GameLogic.Handlers;

namespace GameLogic.Ages
{
    public class ThirdAge : AgeBase
    {
        public override AgesEnum Age => AgesEnum.III;

        public override string CardCompositionFile => "GameLogic.Data.ThirdAgeComposition.csv";

        public ThirdAge(IEventManager eventManager, ICardCompositionFactory cardCompositionFactory, ICardList cardList, IRandomElementReceiver randomElementReceiver) : base(eventManager, cardCompositionFactory, randomElementReceiver.ReceiveRandomElements(cardList?.Cards.Where(card => card.Age == AgesEnum.III).ToArray() ?? [], 20))
        { }
    }
}
