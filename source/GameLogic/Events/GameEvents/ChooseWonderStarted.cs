using GameLogic.Elements;

namespace GameLogic.Events.GameEvents
{
    public class ChooseWonderStarted: GameEvent
    {
        public Player Player { get; }
        public ChooseWonderStarted(Player player)
        {
            Player = player;
        }
    }
}
