using GameLogic.Elements;
using GameLogic.GameStates;
using GameLogic.Interfaces;

namespace GameLogic
{
    public interface IGame
    {
        IGameState? CurrentState { get; }
        IGameContext Context { get; }
        IReadOnlyList<Player> Players { get; }
        void GameLoop();
        void Initialize((string name, IPlayerActionReceiver actionReceiver) player1, (string name, IPlayerActionReceiver actionReceiver) player2, int startingPlayerId = 1);
    }
}
