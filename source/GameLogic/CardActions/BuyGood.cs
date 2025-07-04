using GameLogic.Goods.Factories;

namespace GameLogic.CardActions
{
    public class BuyGood : CardAction
    {
        public int MoneyCost { get; set; }
        public GoodFactory? GoodFactory { get; set; }
    }
}
