using GameLogic.Elements;
using GameLogic.Elements.Wonders;

namespace GameLogic.Events.GameEvents
{
    public class OnWonderChosen: GameEvent
    {
        public Player Player { get; set; }
        public Wonder Wonder { get; set; }

        public OnWonderChosen(Player player, Wonder wonder) { Player = player; Wonder = wonder; }
    }
}
