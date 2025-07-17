using GameLogic.Elements.Goods.Factories;

namespace GameLogic.Elements.Effects
{
    public class BuyGoods : Effect
    {
        public List<BuyGoodItem> BuyGoodItems { get; set; }

        public BuyGoods()
        {
            BuyGoodItems = new List<BuyGoodItem>();
        }

        private BuyGoods(BuyGoods buyGoods)
        {
            BuyGoodItems = buyGoods.BuyGoodItems.Select(b => new BuyGoodItem(b)).ToList();
        }

        public override BuyGoods Clone()
        {
            return new BuyGoods(this);
        }

        public override void Apply()
        {
            throw new NotImplementedException();
        }
    }
}
