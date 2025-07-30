using GameLogic.Ages;

namespace GameLogic.Events
{
    public class OnAgeEnded: EventArgs
    {
        public AgesEnum EndedAge { get; }
        public OnAgeEnded(AgesEnum endedAge)
        {
            EndedAge = endedAge;
        }
    }
}
