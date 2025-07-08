using GameLogic.CardActions;

namespace GameLogic.GameCards
{
    public class YellowCard : Card
    {
        public int Point { get; set; }
        public CardAction? CardActions { get; set; }

        public YellowCard() : base()
        { }
    }
}
