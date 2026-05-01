using SevenWonders.Game.Logic.Elements;
using SevenWonders.Game.Logic.GameStructures;

namespace SevenWonders.Game.Logic.Events.GameEvents
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
