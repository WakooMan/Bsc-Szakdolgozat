using GameLogic.Elements;
using GameLogic.Elements.GameCards;

namespace GameLogic.Events
{
    public class OnCardUnpicked: EventArgs
    {
        public Player Player { get; set; }
        public Card Card { get; set; }

        public OnCardUnpicked(Player player, Card card)
        {
            Player = player;
            Card = card;
        }
    }
}
