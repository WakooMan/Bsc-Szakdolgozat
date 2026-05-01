using SevenWonders.Game.Logic.Elements;
using SevenWonders.Game.Logic.Elements.Military;

namespace SevenWonders.Game.Logic.Events.GameEvents
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