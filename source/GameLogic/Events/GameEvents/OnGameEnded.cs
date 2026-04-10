using GameLogic.Elements;

namespace GameLogic.Events.GameEvents
{
    public class OnGameEnded: GameEvent
    {
        public IDictionary<Player, int> VictoryPoints { get; }

        public OnGameEnded(IDictionary<Player, int> victoryPoints)
        {
            VictoryPoints = victoryPoints;
        }
    }
}
