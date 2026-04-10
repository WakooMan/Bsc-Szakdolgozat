using GameLogic.Elements.Effects;

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

        public override async Task OnBuilt(IGameContext gameContext, int playerId)
        {
            await Point.Apply(gameContext, playerId);
        }

        public override async Task OnDestroyed(IGameContext gameContext, int playerId)
        {
            await Point.Unapply(gameContext, playerId);
        }
    }
}
