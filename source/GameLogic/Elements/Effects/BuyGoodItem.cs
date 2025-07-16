using GameLogic.Elements.Goods.Factories;

namespace GameLogic.Elements.Effects
{
    public class BuyGoodItem
    {
        public int MoneyCost { get; set; }
        public GoodFactory? GoodFactory { get; set; }

        public BuyGoodItem()
        {

        }

        public BuyGoodItem(BuyGoodItem buyGoodItem)
        {
            MoneyCost = buyGoodItem.MoneyCost;
            GoodFactory = buyGoodItem.GoodFactory;
        }
    }
}
