using GameLogic.Elements.Goods;
using GameLogic.Elements.Goods.Products;

namespace GameLogic.Elements.Goods.Factories
{
    public class PapirusFactory : GoodFactory
    {
        public override Good CreateGood()
        {
            return new Papirus();
        }
    }
}
