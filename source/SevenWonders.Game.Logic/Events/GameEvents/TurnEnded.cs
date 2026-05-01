using SevenWonders.Game.Logic.Elements;

namespace SevenWonders.Game.Logic.Events.GameEvents
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
