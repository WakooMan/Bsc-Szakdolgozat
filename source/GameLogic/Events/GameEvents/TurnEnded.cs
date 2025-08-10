using GameLogic.Elements;

namespace GameLogic.Events.GameEvents
{
    public class TurnEnded: GameEvent
    {
        public Player Player { get; }

        public TurnEnded(Player player)
        {
            Player = player;
        }

    }
}
