using GameLogic.Elements.GameCards;
using GameLogic.GameStructures.Factories;

namespace GameLogic.Ages
{
    public class SecondAge : AgeBase
    {
        public override AgesEnum Age => AgesEnum.II;

        public override string CardCompositionFile => Path.Combine(Directory.GetCurrentDirectory(), "Data", "SecondAgeComposition.csv");

        public SecondAge(ICardCompositionFactory cardCompositionFactory, ICardList cardList) : base(cardCompositionFactory, cardList?.Cards.Where(card => card.Age == AgesEnum.II).ToList() ?? null)
        { }
    }
}
