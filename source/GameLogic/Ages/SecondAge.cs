using GameLogic.GameStructures;
using GameLogic.Handlers;
using GameLogic.Handlers.Factories;

namespace GameLogic.Ages
{
    public class SecondAge : AgeBase
    {
        public AgesEnum Age => AgesEnum.II;

        public string CardCompositionFile => Path.Combine(Directory.GetCurrentDirectory(), "Data", "SecondAgeComposition.csv");

        public CardComposition Composition { get; }

        public SecondAge(ICardCompositionFileHandlerFactory cardCompositionFileHandlerFactory)
        {
            ICardCompositionFileHandler cardCompositionFileHandler = cardCompositionFileHandlerFactory.CreateCardCompositionFileHandler(CardCompositionFile);
            Composition = new CardComposition(cardCompositionFileHandler, new List<Elements.GameCards.Card>());
        }
    }
}
