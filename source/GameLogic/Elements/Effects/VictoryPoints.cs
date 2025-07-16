namespace GameLogic.Elements.Effects
{
    public class VictoryPoints : Effect
    {
        public int Points { get; set; }

        public VictoryPoints() { }

        private VictoryPoints(VictoryPoints victoryPoints)
        {
            Points = victoryPoints.Points;
        }

        public override void Apply()
        {
            throw new NotImplementedException();
        }

        public override VictoryPoints Clone()
        {
            return new VictoryPoints(this);
        }
    }
}
