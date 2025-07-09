using GameLogic.GameStructures;
using GameLogic.Handlers;
using GameLogic.Handlers.Factories;

namespace GameLogic.Ages
{
    public class FirstAge : AgeBase
    {
        public AgesEnum Age => AgesEnum.I;

        public string CardCompositionFile => Path.Combine(Directory.GetCurrentDirectory(),"Data","FirstAgeComposition.csv");

        public CardComposition Composition { get; }

        public FirstAge(ICardCompositionFileHandlerFactory cardCompositionFileHandlerFactory)
        {
            ICardCompositionFileHandler cardCompositionFileHandler = cardCompositionFileHandlerFactory.CreateCardCompositionFileHandler(CardCompositionFile);
            Composition = new CardComposition(cardCompositionFileHandler, new List<Elements.GameCards.Card>());
        }
    }
}
