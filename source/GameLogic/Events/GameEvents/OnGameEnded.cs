using GameLogic.Elements;

namespace GameLogic.Events.GameEvents
{
    public class OnGameEnded: GameEvent
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
