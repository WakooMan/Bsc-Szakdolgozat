using GameLogic.Elements.Military;

namespace GameLogic.Events.GameEvents
{
    public class OnMilitaryBoardChanged : GameEvent
    {
        public List<MilitaryField> Fields { get; }

        public OnMilitaryBoardChanged(ICollection<MilitaryField> fields)
        {
            Fields = new List<MilitaryField>(fields);
        }
    }
}
