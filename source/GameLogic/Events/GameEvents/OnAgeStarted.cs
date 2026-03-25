using GameLogic.Ages;

namespace GameLogic.Events.GameEvents
{
    public class OnAgeStarted: GameEvent
    {
        public IAgeBase Age { get; }
        public OnAgeStarted(IAgeBase age)
        {
            Age = age;
        }
    }
}
