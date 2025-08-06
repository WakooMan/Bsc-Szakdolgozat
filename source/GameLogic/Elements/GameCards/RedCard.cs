using GameLogic.Elements.Effects;

namespace GameLogic.Elements.GameCards
{
    public class RedCard : Card
    {
        public Strength Strength { get; set; }
        public RedCard() : base()
        {
            Strength = new Strength();
        }

        private RedCard(RedCard redCard) : base(redCard)
        {
            Strength = redCard.Strength.Clone();
        }

        public override RedCard Clone()
        {
            return new RedCard(this);
        }

        public override int GetStrength()
        {
            return Strength.Points;
        }
    }
}
