using GameLogic.Elements;

namespace GameLogic.Events.GameEvents
{
    public class MilitaryVictory: GameEvent
    {
        public Player Player { get; }

        public MilitaryVictory(Player player)
        {
            Player = player;
        }
    }
}
