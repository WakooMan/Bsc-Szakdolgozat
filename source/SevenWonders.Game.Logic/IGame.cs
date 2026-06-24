using SevenWonders.Game.Logic.Elements;
using SevenWonders.Game.Logic.Interfaces;
using SevenWonders.Common;

namespace SevenWonders.Game.Logic
{
    public interface IGame
    {
        IReadOnlyList<Player> Players { get; }
        bool IsInitialized { get; }
        IGameContext Context { get; }
        void Initialize(IRandomGenerator randomGenerator, (string name, IPlayerActionReceiver actionReceiver) player1, (string name, IPlayerActionReceiver actionReceiver) player2, int startingPlayerId = 1);
        void GameLoop();
        void EndGame();
    }
}
