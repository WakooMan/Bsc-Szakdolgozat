using SevenWonders.Game.Logic.Elements;
using SevenWonders.Game.Logic.Elements.GameCards;

namespace SevenWonders.Game.Logic.Events.GameEvents
{
    public class OnCardPicked : GameEvent
    {
        public Player Player { get; set; }
        public Card Card { get; set; }

        public OnCardPicked(Player player, Card card) { Player = player; Card = card; }
    }
}
