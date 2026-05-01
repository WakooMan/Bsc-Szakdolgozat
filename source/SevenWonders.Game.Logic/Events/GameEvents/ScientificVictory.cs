using SevenWonders.Game.Logic.Elements;

namespace SevenWonders.Game.Logic.Events.GameEvents
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
