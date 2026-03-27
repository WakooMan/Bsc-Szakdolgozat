using GameLogic.GameStructures;

namespace GameLogic.Events.GameEvents
{
    public class CardNodeAvailableEvent: GameEvent
    {
        public ICardNode CardNode { get; }
        public CardNodeAvailableEvent(ICardNode cardNode)
        {
            CardNode = cardNode;
        }
    }
}
