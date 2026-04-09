using GameLogic.Elements.GameCards;

namespace GameLogic.Events.GameEvents
{
    public class OnChooseCards: GameEvent
    {
        public List<Card> Cards { get; }
        public OnChooseCards(ICollection<Card> cards)
        {
            Cards = cards.ToList();
        }
    }
}
