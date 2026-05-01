using SevenWonders.Common;

namespace SevenWonders.Game.Logic.Handlers.Factories
{
    public class CardCompositionFileHandlerFactory : ICardCompositionFileHandlerFactory
    {
        public ICardCompositionFileHandler CreateCardCompositionFileHandler(string compositionFile)
        {
            ArgumentChecker.CheckNullOrEmpty(compositionFile, nameof(compositionFile));

            return new CardCompositionFileHandler(compositionFile);
        }
    }
}
