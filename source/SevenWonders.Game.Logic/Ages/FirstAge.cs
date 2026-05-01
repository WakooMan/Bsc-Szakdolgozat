using SevenWonders.Game.Logic.Elements.GameCards;
using SevenWonders.Game.Logic.Events;
using SevenWonders.Game.Logic.GameStructures.Factories;
using SevenWonders.Common;

namespace SevenWonders.Game.Logic.Ages
{
    public class FirstAge : AgeBase
    {
        public override AgesEnum Age => AgesEnum.I;

        public override string CardCompositionFile => "GameLogic.Data.FirstAgeComposition.csv";

        public FirstAge(IEventManager eventManager, ICardCompositionFactory cardCompositionFactory, ICardList cardList, IRandomGenerator randomGenerator) : base(eventManager, cardCompositionFactory, randomGenerator?.ReceiveRandomElements(cardList?.Cards.Where(card => card.Age == AgesEnum.I).ToArray() ?? [], 20))
        {
            ArgumentChecker.CheckNull(cardList, nameof(cardList));
            ArgumentChecker.CheckNull(randomGenerator, nameof(randomGenerator));
        }
    }
}
