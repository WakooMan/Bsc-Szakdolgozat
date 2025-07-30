using GameLogic.Elements;
using GameLogic.Elements.GameCards;

namespace GameLogic.Events
{
    public class OnCardSold : EventArgs
    {
        public Player Player { get; set; }
        public Card Card { get; set; }
        public int Money { get; set; }

        public OnCardSold(Player player, Card card, int money)
        {
            Player = player;
            Card = card;
            Money = money;
        }
    }
}
