using GameLogic.GameStates;

namespace GameLogic.Elements.Effects
{
    public class BuildFreeFromDroppedCards : Effect
    {
        public BuildFreeFromDroppedCards() { }
        public override void Apply(PlayingState game)
        {
            throw new NotImplementedException();
        }

        public override Effect Clone()
        {
            return new BuildFreeFromDroppedCards();
        }
    }
}
