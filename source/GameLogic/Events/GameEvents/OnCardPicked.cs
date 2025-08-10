using GameLogic.Elements;
using GameLogic.Elements.GameCards;

namespace GameLogic.Events.GameEvents
{
    public class OnCardPicked : GameEvent
    {
        public Player Player { get; set; }
        public Card Card { get; set; }

        public OnCardPicked(Player player, Card card) { Player = player; Card = card; }
    }
}
