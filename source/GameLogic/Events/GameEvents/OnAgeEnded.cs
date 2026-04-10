using GameLogic.Ages;

namespace GameLogic.Events.GameEvents
{
    public class OnAgeEnded: GameEvent
    {
        public IAgeBase Age { get; }
        public OnAgeEnded(IAgeBase age)
        {
            Age = age;
        }
    }
}
