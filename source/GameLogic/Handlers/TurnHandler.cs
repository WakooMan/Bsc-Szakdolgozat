using GameLogic.Elements;

namespace GameLogic.Handlers
{
    public class TurnHandler
    {
        private List<Player> Players { get; set; }
        private int Index { get; set; }
        public Player CurrentPlayer => Players[Index];

        public delegate void PlayerTurnHandler(Player player);
        public event PlayerTurnHandler OnPlayerTurn;

        public void NextPlayer()
        {
            Index = (Index + 1 < Players.Count) ? Index + 1 : 0;
            OnPlayerTurn?.Invoke(CurrentPlayer);
        }

        public TurnHandler(ICollection<Player> players)
        {
            Players = new List<Player>(players);
            Index = 0;
        }
    }
}
