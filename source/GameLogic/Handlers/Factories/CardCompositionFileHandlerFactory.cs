using SevenWonders.Common;
using System.ComponentModel.Composition;

namespace GameLogic.Handlers.Factories
{
    [Export(typeof(ICardCompositionFileHandlerFactory))]
    public class CardCompositionFileHandlerFactory : ICardCompositionFileHandlerFactory
    {
        public ICardCompositionFileHandler CreateCardCompositionFileHandler(string compositionFile)
        {
            ArgumentChecker.CheckNullOrEmpty(compositionFile, nameof(compositionFile));

            return new CardCompositionFileHandler(compositionFile);
        }
    }
}
