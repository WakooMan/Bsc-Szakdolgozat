using GameLogic.Elements.Effects;
using GameLogic.Elements.Goods;

namespace GameLogic.Elements.GameCards
{
    public class BlueCard : Card
    {
        public VictoryPoints Point { get; set; }
        public BlueCard() : base()
        {
            Point = new VictoryPoints();
        }

        private BlueCard(BlueCard blueCard) : base(blueCard)
        {
            Point = blueCard.Point.Clone();
        }

        public override BlueCard Clone()
        {
            return new BlueCard(this);
        }

        public override int GetVictoryPoints(Player player)
        {
            return Point.Points;
        }
    }
}
