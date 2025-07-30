using GameLogic.Elements;

namespace GameLogic.Events
{
    public class OnMilitaryAdvanced: EventArgs
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
