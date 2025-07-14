using GameLogic.Elements.GameCards;
using GameLogic.GameStructures;
using GameLogic.GameStructures.Factories;

namespace GameLogic.Ages
{
    public class FirstAge : AgeBase
    {
        public override AgesEnum Age => AgesEnum.I;

        public override string CardCompositionFile => Path.Combine(Directory.GetCurrentDirectory(),"Data","FirstAgeComposition.csv");

        public FirstAge(ICardCompositionFactory cardCompositionFactory, ICardList cardList) : base(cardCompositionFactory, cardList.Cards.Where(card => card.Age == AgesEnum.I).ToList())
        { }
    }
}
