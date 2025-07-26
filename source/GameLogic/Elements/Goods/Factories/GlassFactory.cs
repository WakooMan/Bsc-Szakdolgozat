using GameLogic.Elements.Goods;
using GameLogic.Elements.Goods.Products;

namespace GameLogic.Elements.Goods.Factories
{
    public class GlassFactory : GoodFactory
    {
        public override Type GoodType => typeof(Glass);

        public override Glass CreateGood()
        {
            return new Glass();
        }
    }
}
