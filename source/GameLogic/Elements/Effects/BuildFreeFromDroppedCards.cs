using GameLogic.Events;
using GameLogic.GameStates;

namespace GameLogic.Elements.Effects
{
    public class BuildFreeFromDroppedCards : Effect
    {
        public BuildFreeFromDroppedCards() { }
        public override void Apply(Player player, IEventManager eventManager)
        {
            throw new NotImplementedException();
        }

        public override Effect Clone()
        {
            return new BuildFreeFromDroppedCards();
        }
    }
}
