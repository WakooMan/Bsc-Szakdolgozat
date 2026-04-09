using GameLogic.Elements.Modifiers;

namespace GameLogic.Events.GameEvents
{
    public class OnChooseDevelopmentsBegin: GameEvent
    {
        public List<Development> Developments { get; }
        public bool FromDeck { get; }

        public OnChooseDevelopmentsBegin(ICollection<Development> developments, bool fromDeck)
        {
            Developments = developments.ToList();
            FromDeck = fromDeck;
        }
    }
}
