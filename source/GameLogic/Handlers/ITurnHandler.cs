using GameLogic.Elements;

namespace GameLogic.Handlers
{
    public interface ITurnHandler
    {
        Player CurrentPlayer { get; }
        Player OpponentPlayer { get; }
        Task NextPlayer();
        Task ForceNewTurn();
        void Initialize(ICollection<Player> players);
    }
}
