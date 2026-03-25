using GameLogic.Elements;
using GameLogic.GameStructures;

namespace GameLogic.Events.GameEvents
{
    public class OnCardUnpicked: GameEvent
    {
        public Player Player { get; set; }
        public ICardNode CardNode { get; set; }

        public OnCardUnpicked(Player player, ICardNode cardNode)
        {
            Player = player;
            CardNode = cardNode;
        }
    }
}
