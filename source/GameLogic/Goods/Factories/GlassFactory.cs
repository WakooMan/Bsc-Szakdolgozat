using GameLogic.Goods.Products;

namespace GameLogic.Goods.Factories
{
    public class GlassFactory : GoodFactory
    {
        public override Good CreateGood()
        {
            return new Glass();
        }
    }
}
