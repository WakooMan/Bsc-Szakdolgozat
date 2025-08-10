using GameLogic.Ages;

namespace GameLogic.Events.GameEvents
{
    public class OnAgeEnded: GameEvent
    {
        public AgesEnum EndedAge { get; }
        public OnAgeEnded(AgesEnum endedAge)
        {
            EndedAge = endedAge;
        }
    }
}
