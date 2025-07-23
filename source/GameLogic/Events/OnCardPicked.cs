using GameLogic.Elements.GameCards;

namespace GameLogic.Events
{
    public class OnCardPicked : EventArgs
    {
        public Card Card { get; set; }

        public OnCardPicked(Card card) { Card = card; }
    }
}
