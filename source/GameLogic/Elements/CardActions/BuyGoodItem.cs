using GameLogic.Elements.Goods.Factories;

namespace GameLogic.Elements.CardActions
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
