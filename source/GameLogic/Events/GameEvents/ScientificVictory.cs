using GameLogic.Elements;

namespace GameLogic.Events.GameEvents
{
    public class ScientificVictory: GameEvent
    {
        public Player Player { get; }

        public ScientificVictory(Player player)
        {
            Player = player;
        }
    }
}
