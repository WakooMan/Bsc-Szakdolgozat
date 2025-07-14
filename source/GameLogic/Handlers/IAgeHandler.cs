using GameLogic.Ages;

namespace GameLogic.Handlers
{
    public interface IAgeHandler
    {
        IAgeBase CurrentAge { get; }

        delegate void AgeChangedEventHandler(AgesEnum age);

        event AgeChangedEventHandler HandleAgeChanged;

        bool NextAge();
    }
}
