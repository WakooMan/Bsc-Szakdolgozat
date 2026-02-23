using GameLogic.Elements;
using GameLogic.GameStates;

namespace GameLogic
{
    public interface IGame
    {
        IGameState? CurrentState { get; }
        IReadOnlyList<Player> Players { get; }
        void GameLoop();
        void Initialize(string player1, string player2);
    }
}
