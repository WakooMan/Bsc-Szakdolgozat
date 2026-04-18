namespace GameLogic.Elements.Effects
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

        // TODO: At the end of the turn military strength of players should be compared and the pawn should be moved accordingly.

        public override Task OnCalculatePlayerProperties(PlayerProperties playerProperties)
        {
            playerProperties.Strength += Points;
            return Task.CompletedTask;
        }
    }
}
