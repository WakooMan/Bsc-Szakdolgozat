using GameLogic.Elements;

namespace GameLogic.Events
{
    public class TurnStarted : EventArgs
    {
        public Player Player { get; }
        public TurnStarted(Player player)
        {
            Player = player;
        }
    }
}
