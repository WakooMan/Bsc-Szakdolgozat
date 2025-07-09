using GameLogic.Elements.Goods;
using GameLogic.Elements.Goods.Products;

namespace GameLogic.Elements.Goods.Factories
{
    public class GlassFactory : GoodFactory
    {
        public override Good CreateGood()
        {
            return new Glass();
        }
    }
}
