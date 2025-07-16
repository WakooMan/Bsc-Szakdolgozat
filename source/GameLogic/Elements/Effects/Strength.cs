namespace GameLogic.Elements.Effects
{
    public class Strength : Effect
    {
        public int Points { get; set; }

        public Strength() { }
        private Strength(Strength strength)
        {
            Points = strength.Points;
        }

        public override void Apply()
        {
            throw new NotImplementedException();
        }

        public override Strength Clone()
        {
            return new Strength(this);
        }
    }
}
