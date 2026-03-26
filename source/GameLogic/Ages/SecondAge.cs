using GameLogic.Elements.GameCards;
using GameLogic.Events;
using GameLogic.GameStructures.Factories;

namespace GameLogic.Ages
{
    public class SecondAge : AgeBase
    {
        public override AgesEnum Age => AgesEnum.II;

        public override string CardCompositionFile => "GameLogic.Data.SecondAgeComposition.csv";

        public SecondAge(IEventManager eventManager, ICardCompositionFactory cardCompositionFactory, ICardList cardList) : base(eventManager, cardCompositionFactory, cardList?.Cards.Where(card => card.Age == AgesEnum.II).Take(20).ToList() ?? null)
        { }
    }
}
