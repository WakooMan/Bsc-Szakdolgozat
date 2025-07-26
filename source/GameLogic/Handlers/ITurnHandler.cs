using GameLogic.Elements;

namespace GameLogic.Handlers
{
    public interface ITurnHandler
    {
        public delegate void PlayerTurnHandler(Player player);
        Player CurrentPlayer { get; }
        Player OpponentPlayer { get; }
        void NextPlayer();
        void ForceNewTurn();
    }
}
