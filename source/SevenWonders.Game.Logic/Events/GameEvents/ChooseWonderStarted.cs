using SevenWonders.Game.Logic.Elements;

namespace SevenWonders.Game.Logic.Events.GameEvents
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
