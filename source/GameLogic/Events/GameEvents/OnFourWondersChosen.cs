using GameLogic.Elements.Wonders;

namespace GameLogic.Events.GameEvents
{
    public class OnFourWondersChosen: GameEvent
    {
        public ICollection<Wonder> Wonders { get; }

        public OnFourWondersChosen(ICollection<Wonder> wonders)
        {
            Wonders = wonders;
        }
    }
}
