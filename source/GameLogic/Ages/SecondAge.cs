using GameLogic.Elements.GameCards;
using GameLogic.Events;
using GameLogic.GameStructures.Factories;
using SevenWonders.Common;

namespace GameLogic.Ages
{
    public class SecondAge : AgeBase
    {
        public override AgesEnum Age => AgesEnum.II;

        public override string CardCompositionFile => "GameLogic.Data.SecondAgeComposition.csv";

        public SecondAge(IEventManager eventManager, ICardCompositionFactory cardCompositionFactory, ICardList cardList, IRandomGenerator randomGenerator) : base(eventManager, cardCompositionFactory, randomGenerator.ReceiveRandomElements(cardList?.Cards.Where(card => card.Age == AgesEnum.II).ToArray() ?? [], 20))
        { }
    }
}
