using GameLogic.GameStructures;

namespace GameLogic.Ages
{
    public abstract class AgeBase
    {
        public abstract AgesEnum Age { get; }
        public abstract string CardCompositionFile { get; }
        public abstract CardComposition Composition { get; }
        public bool IsAgeOver => Composition.AvailableCards.Count < 0;
    }
}
