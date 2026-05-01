using SevenWonders.Game.Logic.Elements.Military;

namespace SevenWonders.Game.Logic.Events.GameEvents
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
