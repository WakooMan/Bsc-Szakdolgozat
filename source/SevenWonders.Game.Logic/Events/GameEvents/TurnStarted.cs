using SevenWonders.Game.Logic.Elements;

namespace SevenWonders.Game.Logic.Events.GameEvents
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
