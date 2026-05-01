using SevenWonders.Game.Logic.GameStructures;

namespace SevenWonders.Game.Logic.Events.GameEvents
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
