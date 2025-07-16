namespace GameLogic.Elements.GameCards
{
    public class RedCard : Card
    {
        public int Strength { get; set; }
        public RedCard() : base()
        { }

        private RedCard(RedCard redCard) : base(redCard)
        {
            Strength = redCard.Strength;
        }

        public override ICard Clone()
        {
            return new RedCard(this);
        }
    }
}
