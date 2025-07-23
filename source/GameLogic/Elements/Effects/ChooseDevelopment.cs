using GameLogic.GameStates;

namespace GameLogic.Elements.Effects
{
    public class ChooseDevelopment : Effect
    {
        public ChooseDevelopment()
        {

        }
        public override void Apply(PlayingState game)
        {
            throw new NotImplementedException();
        }

        public override Effect Clone()
        {
            return new ChooseDevelopment();
        }
    }
}
