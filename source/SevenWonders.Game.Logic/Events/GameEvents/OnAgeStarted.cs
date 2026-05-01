using SevenWonders.Game.Logic.Ages;

namespace SevenWonders.Game.Logic.Events.GameEvents
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
