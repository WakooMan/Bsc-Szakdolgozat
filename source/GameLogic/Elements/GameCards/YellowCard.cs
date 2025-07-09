using GameLogic.Elements.CardActions;

namespace GameLogic.Elements.GameCards
{
    public class YellowCard : Card
    {
        public int Point { get; set; }
        public CardAction? CardActions { get; set; }

        public YellowCard() : base()
        { }
    }
}
