using GameLogic.Elements;
using GameLogic.Events;
using GameLogic.GameStates;

namespace GameLogic
{
    public interface IGame
    {
        IGameState? CurrentState { get; }
        IGameContext Context { get; }
        IReadOnlyList<Player> Players { get; }
        void GameLoop();
        void Initialize(string player1, string player2);
    }
}
