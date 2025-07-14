using GameLogic.Elements;
using SevenWonders.Common;
using static GameLogic.Handlers.ITurnHandler;

namespace GameLogic.Handlers
{
    public class TurnHandler: ITurnHandler
    {
        private List<Player> Players { get; set; }
        private int Index { get; set; }
        public Player CurrentPlayer => Players[Index];

        public event PlayerTurnHandler OnPlayerTurn;

        public void NextPlayer()
        {
            Index = (Index + 1 < Players.Count) ? Index + 1 : 0;
            OnPlayerTurn?.Invoke(CurrentPlayer);
        }

        public TurnHandler(ICollection<Player> players)
        {
            ArgumentChecker.CheckNull(players, nameof(players));
            ArgumentChecker.CheckPredicateForArgument(() => players.Count < 2, "Number of players must be at least 2!");

            Players = new List<Player>(players);
            Index = 0;
        }
    }
}
