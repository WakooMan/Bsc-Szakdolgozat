using GameLogic.Ages;
using SevenWonders.Common;

namespace GameLogic.Handlers
{
    public interface IAgeHandler
    {
        IAgeBase CurrentAge { get; }
        Task Initialize(IRandomGenerator? randomGenerator);
        Task<bool> NextAge();
    }
}
