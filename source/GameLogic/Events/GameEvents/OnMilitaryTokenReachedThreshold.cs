using GameLogic.Elements;
using GameLogic.Elements.Military;

namespace GameLogic.Events.GameEvents
{
    public class OnMilitaryTokenReachedThreshold : GameEvent
    {
        public List<MilitaryCard> MilitaryCards { get; }
        public MilitaryCard? CurrentMilitaryCard { get; }
        public MilitaryCard? PreviousMilitaryCard { get; }
        public Player Player { get; }

        public OnMilitaryTokenReachedThreshold(Player player, List<MilitaryCard> militaryCards, MilitaryCard? previousMilitaryCard, MilitaryCard? currentMilitaryCard)
        {
            MilitaryCards = militaryCards;
            Player = player;
            CurrentMilitaryCard = currentMilitaryCard;
            PreviousMilitaryCard = previousMilitaryCard;
        }
    }
}