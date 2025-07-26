using GameLogic.Elements.Goods;
using GameLogic.Elements.Goods.Products;

namespace GameLogic.Elements.Goods.Factories
{
    public class PapirusFactory : GoodFactory
    {
        public override Type GoodType => typeof(Papirus);

        public override Papirus CreateGood()
        {
            return new Papirus();
        }
    }
}
