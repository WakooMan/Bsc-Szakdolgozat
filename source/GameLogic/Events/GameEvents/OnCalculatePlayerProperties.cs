using GameLogic.Elements;

namespace GameLogic.Events.GameEvents
{
    public class OnCalculatePlayerProperties: GameEvent
    {
        public PlayerProperties PlayerProperties { get; }

        public OnCalculatePlayerProperties(PlayerProperties playerProperties)
        {
            PlayerProperties = playerProperties;
        }
    }
}
