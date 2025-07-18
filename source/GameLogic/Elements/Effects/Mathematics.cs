namespace GameLogic.Elements.Effects
{
    public class Mathematics : Effect
    {
        public VictoryPoints VictoryPointsPerDevelopment { get; set; }

        public Mathematics() { }

        private Mathematics(Mathematics mathematics)
        {
            VictoryPointsPerDevelopment = mathematics.VictoryPointsPerDevelopment.Clone();
        }

        public override void Apply()
        {
            throw new NotImplementedException();
        }

        public override Mathematics Clone()
        {
            return new Mathematics(this);
        }
    }
}
