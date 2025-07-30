using GameLogic.Elements.Military;

namespace GameLogic.Events
{
    public class OnMilitaryTokenReachedThreshold : EventArgs
    {
        public List<MilitaryCard> MilitaryCards { get; set; }

        public OnMilitaryTokenReachedThreshold(List<MilitaryCard> militaryCards)
        {
            MilitaryCards = militaryCards;
        }
    }
}