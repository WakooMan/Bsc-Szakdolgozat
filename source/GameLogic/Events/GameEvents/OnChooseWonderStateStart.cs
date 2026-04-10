using GameLogic.Elements.Wonders;

namespace GameLogic.Events.GameEvents
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
