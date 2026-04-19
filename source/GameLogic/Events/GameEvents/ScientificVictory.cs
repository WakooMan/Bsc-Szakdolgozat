using GameLogic.Elements;

namespace GameLogic.Events.GameEvents
{
    public class ScientificVictory: GameEvent
    {
        public PlayerProperties PlayerProperties { get; }

        public ScientificVictory(PlayerProperties playerProperties)
        {
            PlayerProperties = playerProperties;
        }
    }
}
