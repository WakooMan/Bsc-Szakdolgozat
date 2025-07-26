using GameLogic.Elements;
using GameLogic.Elements.GameCards;

namespace GameLogic.Events
{
    public class OnCardBuilt: EventArgs
    {
        public Card Card { get; }
        public Player Builder { get; }

        public OnCardBuilt(Card card, Player builder)
        {
            Card = card;
            Builder = builder;
        }
    }
}
