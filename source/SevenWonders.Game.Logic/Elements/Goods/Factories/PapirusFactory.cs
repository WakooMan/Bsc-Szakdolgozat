using SevenWonders.Game.Logic.Elements.Goods;
using SevenWonders.Game.Logic.Elements.Goods.Products;

namespace SevenWonders.Game.Logic.Elements.Goods.Factories
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
