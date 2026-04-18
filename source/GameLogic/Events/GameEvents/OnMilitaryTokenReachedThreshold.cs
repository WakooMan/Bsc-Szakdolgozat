using GameLogic.Elements;
using GameLogic.Elements.Military;

namespace GameLogic.Events.GameEvents
{
    public class OnMilitaryTokenReachedThreshold : GameEvent
    {
        public List<MilitaryCard> MilitaryCards { get; }
        public MilitaryCard? CurrentMilitaryCard { get; }
        public MilitaryCard? PreviousMilitaryCard { get; }

        public OnMilitaryTokenReachedThreshold(List<MilitaryCard> militaryCards, MilitaryCard? previousMilitaryCard, MilitaryCard? currentMilitaryCard)
        {
            MilitaryCards = militaryCards;
            CurrentMilitaryCard = currentMilitaryCard;
            PreviousMilitaryCard = previousMilitaryCard;
        }
    }
}