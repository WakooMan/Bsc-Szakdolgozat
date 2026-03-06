using GameLogic.Elements.Wonders;

namespace GameLogic.Events.GameEvents
{
    public class OnChooseWonderStateEnd: GameEvent
    {
        public ICollection<Wonder> Wonders { get; }

        public OnChooseWonderStateEnd(ICollection<Wonder> wonders)
        {
            Wonders = wonders;
        }
    }
}
