using GameLogic.Elements;
using GameLogic.Elements.Modifiers;
using GameLogic.Elements.Wonders;
using GameLogic.GameStates;

namespace GameLogic
{
    public interface IGame
    {
        IGameState? CurrentState { get; }
        IReadOnlyList<Player> Players { get; }
        void GameLoop();
        void Initialize(string player1, string player2, ICollection<Wonder> wonders, ICollection<Development> developments);
    }
}
