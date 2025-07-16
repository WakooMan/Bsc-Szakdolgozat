using GameLogic.Elements.CardActions;

namespace GameLogic.Elements.GameCards
{
    public class YellowCard : Card
    {
        public int Point { get; set; }
        public CardAction Action { get; set; }

        public YellowCard() : base()
        { }

        private YellowCard(YellowCard yellowCard) : base(yellowCard)
        {
            Point = yellowCard.Point;
            Action = yellowCard.Action.Clone();
        }

        public override ICard Clone()
        {
            return new YellowCard(this);
        }
    }
}
