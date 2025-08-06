using GameLogic.Elements.GameCards;
using GameLogic.GameStructures.Factories;

namespace GameLogic.Ages
{
    public class ThirdAge : AgeBase
    {
        public override AgesEnum Age => AgesEnum.III;

        public override string CardCompositionFile => Path.Combine(Directory.GetCurrentDirectory(), "Data", "ThirdAgeComposition.csv");

        public ThirdAge(ICardCompositionFactory cardCompositionFactory, ICardList cardList) : base(cardCompositionFactory, cardList?.Cards.Where(card => card.Age == AgesEnum.III).ToList() ?? [])
        { }
    }
}
