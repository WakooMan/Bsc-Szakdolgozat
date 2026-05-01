using SevenWonders.Game.Logic.Elements;
using SevenWonders.Game.Logic.Elements.GameCards;

namespace SevenWonders.Game.Logic.Events.GameEvents
{
    public class OnCardDestroyed: GameEvent
    {
        public Player Player { get; set; }
        public Card Card { get; set; }

        public OnCardDestroyed(Player player, Card card)
        {
            Player = player;
            Card = card;
        }
    }
}
