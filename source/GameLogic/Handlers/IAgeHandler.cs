using GameLogic.Ages;

namespace GameLogic.Handlers
{
    public interface IAgeHandler
    {
        IAgeBase? CurrentAge { get; }
        void Initialize();
        bool NextAge();
    }
}
