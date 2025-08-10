using GameLogic.Elements;
using GameLogic.Elements.GameCards;

namespace GameLogic.Events.GameEvents
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
