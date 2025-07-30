using GameLogic.Elements;
using GameLogic.Elements.GameCards;
using GameLogic.Elements.Wonders;

namespace GameLogic.Events
{
    public class OnWonderBuilt: EventArgs
    {
        public Wonder Wonder { get; }
        public Player Builder { get; }
        public Card Card { get; }

        public OnWonderBuilt(Player builder, Card card, Wonder wonder)
        {
            Builder = builder;
            Wonder = wonder;
            Card = card;
        }
    }
}
