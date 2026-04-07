using GameLogic.Elements.Modifiers;

namespace GameLogic.Events.GameEvents
{
    public class OnChooseDevelopmentsEnd: GameEvent
    {
        public List<Development> Developments { get; }

        public OnChooseDevelopmentsEnd(ICollection<Development> developments)
        {
            Developments = developments.ToList();
        }
    }
}
