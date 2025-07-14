using GameLogic.GameStructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLogic.Ages
{
    public interface IAgeBase
    {
        AgesEnum Age { get; }
        string CardCompositionFile { get; }
        ICardComposition Composition { get; }
        public bool IsAgeOver { get; }
    }
}
