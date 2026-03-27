using GameLogic.Ages;

namespace GameLogic.Handlers
{
    public interface IAgeHandler
    {
        IAgeBase CurrentAge { get; }
        Task Initialize();
        Task<bool> NextAge();
    }
}
