using SevenWonders.Game.Logic.Elements.Goods;
using SevenWonders.Game.Logic.Elements.Goods.Products;

namespace SevenWonders.Game.Logic.Elements.Goods.Factories
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
