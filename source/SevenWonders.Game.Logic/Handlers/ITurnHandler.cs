using SevenWonders.Game.Logic.Elements;

namespace SevenWonders.Game.Logic.Handlers
{
    public interface ITurnHandler
    {
        Player CurrentPlayer { get; }
        Player OpponentPlayer { get; }
        bool NewTurnForced { get; }
        Player GetPlayer(int index);

        void NextPlayer();
        void ForceNewTurn();
        void Initialize(ICollection<Player> players);
    }
}
