using GameLogic.Elements.GameCards;

namespace GameLogic.Events.GameEvents
{
    public class OnCardChosen: GameEvent
    {
        public List<Card> DroppedCards { get; }
        public OnCardChosen(ICollection<Card> droppedCards)
        {
            DroppedCards = droppedCards.ToList();
        }
    }
}
