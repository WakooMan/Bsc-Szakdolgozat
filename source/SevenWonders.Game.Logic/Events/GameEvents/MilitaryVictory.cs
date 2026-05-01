using SevenWonders.Game.Logic.Elements;

namespace SevenWonders.Game.Logic.Events.GameEvents
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
