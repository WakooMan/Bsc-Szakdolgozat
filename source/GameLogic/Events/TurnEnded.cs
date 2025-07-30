using GameLogic.Elements;

namespace GameLogic.Events
{
    public class TurnEnded: EventArgs
    {
        public Player Player { get; }

        public TurnEnded(Player player)
        {
            Player = player;
        }

    }
}
