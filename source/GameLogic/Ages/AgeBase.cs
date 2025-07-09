using GameLogic.GameStructures;

namespace GameLogic.Ages
{
    public interface AgeBase
    {
        public AgesEnum Age { get; }
        public string CardCompositionFile { get; }
        CardComposition Composition { get; }
    }
}
