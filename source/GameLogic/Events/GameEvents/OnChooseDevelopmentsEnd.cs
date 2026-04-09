using GameLogic.Elements.Modifiers;

namespace GameLogic.Events.GameEvents
{
    public class OnChooseDevelopmentsEnd: GameEvent
    {
        public List<Development> Developments { get; }
        public bool FromDeck { get; }

        public OnChooseDevelopmentsEnd(ICollection<Development> developments, bool fromDeck)
        {
            Developments = developments.ToList();
            FromDeck = fromDeck;
        }
    }
}
