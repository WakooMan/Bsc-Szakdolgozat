using GameLogic.Elements;

namespace GameLogic.Handlers
{
    public interface ITurnHandler
    {
        Player CurrentPlayer { get; }
        Player OpponentPlayer { get; }
        void NextPlayer();
        void ForceNewTurn();
        void SetPlayers(ICollection<Player> players);
    }
}
