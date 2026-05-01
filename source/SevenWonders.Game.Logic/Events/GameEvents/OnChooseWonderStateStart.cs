using SevenWonders.Game.Logic.Elements.Wonders;

namespace SevenWonders.Game.Logic.Events.GameEvents
{
    public class OnChooseWonderStateStart : GameEvent
    {
        public ICollection<Wonder> Wonders { get; }

        public OnChooseWonderStateStart(ICollection<Wonder> wonders)
        {
            Wonders = wonders;
        }
    }
}
