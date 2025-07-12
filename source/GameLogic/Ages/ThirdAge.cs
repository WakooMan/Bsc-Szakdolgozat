using GameLogic.GameStructures;
using GameLogic.Handlers;
using GameLogic.Handlers.Factories;

namespace GameLogic.Ages
{
    public class ThirdAge : AgeBase
    {
        public override AgesEnum Age => AgesEnum.III;

        public override string CardCompositionFile => Path.Combine(Directory.GetCurrentDirectory(), "Data", "ThirdAgeComposition.csv");
        public override CardComposition Composition { get; }

        public ThirdAge(ICardCompositionFileHandlerFactory cardCompositionFileHandlerFactory)
        {
            ICardCompositionFileHandler cardCompositionFileHandler = cardCompositionFileHandlerFactory.CreateCardCompositionFileHandler(CardCompositionFile);
            Composition = new CardComposition(cardCompositionFileHandler, new List<Elements.GameCards.Card>());
        }
    }
}
