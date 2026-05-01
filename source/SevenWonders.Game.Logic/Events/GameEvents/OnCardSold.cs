using SevenWonders.Game.Logic.Elements;
using SevenWonders.Game.Logic.Elements.GameCards;

namespace SevenWonders.Game.Logic.Events.GameEvents
{
    public class OnCardSold : GameEvent
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
