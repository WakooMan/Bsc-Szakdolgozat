using GameLogic.GameStates;

namespace GameLogic.Elements.Effects
{
    public class PlusStrengthOnRedCardBuild : Effect
    {
        public Strength AdditionalStrength { get; set; }

        public PlusStrengthOnRedCardBuild() { }

        private PlusStrengthOnRedCardBuild(PlusStrengthOnRedCardBuild plusStrengthOnRedCardBuild)
        {
            AdditionalStrength = plusStrengthOnRedCardBuild.AdditionalStrength.Clone();
        }

        public override void Apply(PlayingState game)
        {
            throw new NotImplementedException();
        }

        public override PlusStrengthOnRedCardBuild Clone()
        {
            return new PlusStrengthOnRedCardBuild(this);
        }
    }
}
