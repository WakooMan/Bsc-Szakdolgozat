using GameLogic.Elements.Effects;

namespace GameLogic.Elements.GameCards
{
    public class BlueCard : Card
    {
        public VictoryPoints Point { get; set; }
        public BlueCard() : base()
        { }

        private BlueCard(BlueCard blueCard) : base(blueCard)
        {
            Point = blueCard.Point.Clone();
        }

        public override BlueCard Clone()
        {
            return new BlueCard(this);
        }
    }
}
