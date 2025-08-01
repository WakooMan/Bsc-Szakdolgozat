using GameLogic.Elements;
using GameLogic.Elements.GameCards;

namespace GameLogic.Events.GameEvents
{
    public class OnCardBuilt: GameEvent
    {
        public Card Card { get; }
        public Player Builder { get; }
        public int BuildCost { get; }
        public bool ChainBuildUsed { get; }

        public OnCardBuilt(Card card, Player builder, int buildCost, bool chainBuildUsed)
        {
            Card = card;
            Builder = builder;
            BuildCost = buildCost;
            ChainBuildUsed = chainBuildUsed;
        }
    }
}
