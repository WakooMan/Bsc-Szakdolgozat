using GameLogic.Elements;

namespace GameLogic.Events.GameEvents
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
