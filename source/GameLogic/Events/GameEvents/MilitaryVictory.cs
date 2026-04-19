using GameLogic.Elements;

namespace GameLogic.Events.GameEvents
{
    public class MilitaryVictory: GameEvent
    {
        public PlayerProperties PlayerProperties { get; }

        public MilitaryVictory(PlayerProperties playerProperties)
        {
            PlayerProperties = playerProperties;
        }
    }
}
