using SevenWonders.Game.Logic.Elements.Wonders;

namespace SevenWonders.Game.Logic.Events.GameEvents
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
