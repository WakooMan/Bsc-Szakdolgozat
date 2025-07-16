namespace GameLogic.Elements.GameCards
{
    public class BlueCard : Card
    {
        public int Point { get; set; }
        public BlueCard() : base()
        { }

        private BlueCard(BlueCard blueCard) : base(blueCard)
        {
            Point = blueCard.Point;
        }

        public override ICard Clone()
        {
            return new BlueCard(this);
        }
    }
}
