using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SevenWonders.Game.Logic.Handlers.Factories
{
    public interface ICardCompositionFileHandlerFactory
    {
        ICardCompositionFileHandler CreateCardCompositionFileHandler(string compositionFile);
    }
}
