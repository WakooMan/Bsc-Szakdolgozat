using GameLogic.Elements;

namespace GameLogic.Events.GameEvents
{
    public class TurnStarted : GameEvent
    {
        public Player Player { get; }
        public TurnStarted(Player player)
        {
            Player = player;
        }
    }
}
