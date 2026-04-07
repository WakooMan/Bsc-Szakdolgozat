using GameLogic.Elements;

namespace GameLogic.Events.GameEvents
{
    public class OnGameStarted: GameEvent
    {
        public ICollection<Player> Players { get; }
        public IGameContext GameContext { get; }

        public OnGameStarted(ICollection<Player> players, IGameContext gameContext)
        {
            Players = players;
            GameContext = gameContext;
        }
    }
}
