using SevenWonders.Game.Logic.Ages;

namespace SevenWonders.Game.Logic.Events.GameEvents
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
