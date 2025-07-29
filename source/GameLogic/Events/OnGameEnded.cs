using GameLogic.Elements;

namespace GameLogic.Events
{
    public class OnGameEnded: EventArgs
    {
        public Dictionary<Player, int> VictoryPoints { get; }

        public OnGameEnded(ICollection<Player> players)
        {
            VictoryPoints = new Dictionary<Player, int>();
            foreach (Player player in players)
            {
                VictoryPoints.Add(player, 0);
            }

        }
    }
}
