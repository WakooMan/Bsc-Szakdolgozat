using SevenWonders.Game.Logic.Elements.GameCards;
using SevenWonders.Game.Logic.Events;
using SevenWonders.Game.Logic.GameStructures.Factories;
using SevenWonders.Common;

namespace SevenWonders.Game.Logic.Ages
{
    public class ThirdAge : AgeBase
    {
        public override AgesEnum Age => AgesEnum.III;

        public override string CardCompositionFile => "SevenWonders.Game.Logic.Data.ThirdAgeComposition.csv";

        public ThirdAge(IEventManager eventManager, ICardCompositionFactory cardCompositionFactory, ICardList cardList, IRandomGenerator randomGenerator) : base(eventManager, cardCompositionFactory, randomGenerator?.ReceiveRandomElements(cardList?.Cards.Where(card => card.Age == AgesEnum.III).ToArray() ?? [], 20))
        {
            ArgumentChecker.CheckNull(cardList, nameof(cardList));
            ArgumentChecker.CheckNull(randomGenerator, nameof(randomGenerator));
        }
    }
}
