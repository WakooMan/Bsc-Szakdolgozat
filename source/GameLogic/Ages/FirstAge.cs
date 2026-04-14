using GameLogic.Elements.GameCards;
using GameLogic.Events;
using GameLogic.GameStructures.Factories;
using SevenWonders.Common;

namespace GameLogic.Ages
{
    public class FirstAge : AgeBase
    {
        public override AgesEnum Age => AgesEnum.I;

        public override string CardCompositionFile => "GameLogic.Data.FirstAgeComposition.csv";

        public FirstAge(IEventManager eventManager, ICardCompositionFactory cardCompositionFactory, ICardList cardList, IRandomGenerator randomGenerator) : base(eventManager, cardCompositionFactory, randomGenerator.ReceiveRandomElements(cardList?.Cards.Where(card => card.Age == AgesEnum.I).ToArray() ?? [], 20))
        { }
    }
}
