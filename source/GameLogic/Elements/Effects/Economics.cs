using GameLogic.GameStates;

namespace GameLogic.Elements.Effects
{
    public class Economics : Effect
    {
        public Economics() { }
        public override void Apply(PlayingState game)
        {
            throw new NotImplementedException();
        }

        public override Economics Clone()
        {
            return new Economics();
        }
    }
}
