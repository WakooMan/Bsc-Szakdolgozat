using SevenWonders.Game.Logic.Elements.Effects;

namespace SevenWonders.Game.Logic.Elements.GameCards
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

        public override void OnCalculatePlayerProperties(PlayerProperties playerProperties)
        {
            Point.OnCalculatePlayerProperties(playerProperties);
        }
    }
}
