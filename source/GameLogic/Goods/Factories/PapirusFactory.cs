using GameLogic.Goods.Products;

namespace GameLogic.Goods.Factories
{
    public class PapirusFactory : GoodFactory
    {
        public override Good CreateGood()
        {
            return new Papirus();
        }
    }
}
