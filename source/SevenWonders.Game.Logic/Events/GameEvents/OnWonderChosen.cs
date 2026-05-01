using SevenWonders.Game.Logic.Elements;
using SevenWonders.Game.Logic.Elements.Wonders;

namespace SevenWonders.Game.Logic.Events.GameEvents
{
    public class OnWonderChosen: GameEvent
    {
        public Player Player { get; set; }
        public Wonder Wonder { get; set; }

        public OnWonderChosen(Player player, Wonder wonder) { Player = player; Wonder = wonder; }
    }
}
