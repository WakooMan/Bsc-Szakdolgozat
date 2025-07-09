using GameLogic.Elements.Goods.Factories;

namespace GameLogic.Elements.CardActions
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
