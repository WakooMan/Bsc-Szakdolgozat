using GameLogic.Elements.Modifiers;

namespace GameLogic.Events.GameEvents
{
    public class OnChooseDevelopmentsBegin: GameEvent
    {
        public List<Development> Developments { get; }

        public OnChooseDevelopmentsBegin(ICollection<Development> developments)
        {
            Developments = developments.ToList();
        }
    }
}
