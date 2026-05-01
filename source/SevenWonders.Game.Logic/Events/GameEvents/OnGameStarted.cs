using SevenWonders.Game.Logic.Elements;

namespace SevenWonders.Game.Logic.Events.GameEvents
{
    public class OnGameStarted: GameEvent
    {
        public ICollection<Player> Players { get; }

        public OnGameStarted(ICollection<Player> players)
        {
            Players = players;
        }
    }
}
