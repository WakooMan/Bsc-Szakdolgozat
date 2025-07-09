namespace GameLogic.Handlers.Factories
{
    public class CardCompositionFileHandlerFactory : ICardCompositionFileHandlerFactory
    {
        public ICardCompositionFileHandler CreateCardCompositionFileHandler(string compositionFile)
        {
            return new CardCompositionFileHandler(compositionFile);
        }
    }
}
