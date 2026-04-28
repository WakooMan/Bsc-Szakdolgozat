using GameLogic.Ages;
using SevenWonders.Common;

namespace GameLogic.Handlers
{
    public interface IAgeHandler
    {
        IAgeBase CurrentAge { get; }
        void Initialize(IRandomGenerator? randomGenerator);
        bool NextAge();
    }
}
