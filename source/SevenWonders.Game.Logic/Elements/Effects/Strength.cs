namespace SevenWonders.Game.Logic.Elements.Effects
{
    public class Strength : Effect
    {
        public int Points { get; set; }

        public Strength()
        {
            Points = 0;
        }
        private Strength(Strength strength)
        {
            Points = strength.Points;
        }

        public override Strength Clone()
        {
            return new Strength(this);
        }

        public override void OnCalculatePlayerProperties(PlayerProperties playerProperties)
        {
            playerProperties.Strength += Points;
        }
    }
}
