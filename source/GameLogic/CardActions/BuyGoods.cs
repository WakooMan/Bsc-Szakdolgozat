using GameLogic.Goods.Factories;

namespace GameLogic.CardActions
{
    public class BuyGoods : CardAction
    {
        public List<BuyGoodItem> BuyGoodItems { get; set; }

        public BuyGoods() : base()
        {
            BuyGoodItems = new List<BuyGoodItem>();
        }
    }
}
