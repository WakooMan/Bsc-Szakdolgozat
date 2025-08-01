using GameLogic.Elements.Military;

namespace GameLogic.Events.GameEvents
{
    public class OnMilitaryTokenReachedThreshold : GameEvent
    {
        public List<MilitaryCard> MilitaryCards { get; set; }

        public OnMilitaryTokenReachedThreshold(List<MilitaryCard> militaryCards)
        {
            MilitaryCards = militaryCards;
        }
    }
}