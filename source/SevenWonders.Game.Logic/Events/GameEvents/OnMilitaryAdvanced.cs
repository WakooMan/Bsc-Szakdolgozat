using SevenWonders.Game.Logic.Elements;

namespace SevenWonders.Game.Logic.Events.GameEvents
{
    public class OnMilitaryAdvanced: GameEvent
    {
        public Player Player { get; set; }
        public int Advancement { get; set; }

        public OnMilitaryAdvanced(Player player, int advancement)
        {
            Player = player;
            Advancement = advancement;
        }
    }
}
