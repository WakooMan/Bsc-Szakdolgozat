using SevenWonders.Game.Logic.Elements;
using SevenWonders.Game.Logic.Elements.GameCards;
using SevenWonders.Game.Logic.Elements.Wonders;

namespace SevenWonders.Game.Logic.Events.GameEvents
{
    public class OnWonderBuilt: GameEvent
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
