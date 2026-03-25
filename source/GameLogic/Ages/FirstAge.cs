using GameLogic.Elements.GameCards;
using GameLogic.Events;
using GameLogic.GameStructures.Factories;

namespace GameLogic.Ages
{
    public class FirstAge : AgeBase
    {
        public override AgesEnum Age => AgesEnum.I;

        public override string CardCompositionFile => Path.Combine(Directory.GetCurrentDirectory(),"Data","FirstAgeComposition.csv");

        public FirstAge(IEventManager eventManager, ICardCompositionFactory cardCompositionFactory, ICardList cardList) : base(eventManager, cardCompositionFactory, cardList?.Cards.Where(card => card.Age == AgesEnum.I).Take(20).ToList() ?? null)
        { }
    }
}
