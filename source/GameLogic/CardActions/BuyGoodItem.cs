using GameLogic.Goods.Factories;

namespace GameLogic.CardActions
{
    public class BuyGoodItem
    {
        public int MoneyCost { get; set; }
        public GoodFactory? GoodFactory { get; set; }

        public BuyGoodItem()
        {

        }
    }
}
