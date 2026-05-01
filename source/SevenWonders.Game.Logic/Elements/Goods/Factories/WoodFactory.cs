using SevenWonders.Game.Logic.Elements.Goods;
using SevenWonders.Game.Logic.Elements.Goods.Resources;

namespace SevenWonders.Game.Logic.Elements.Goods.Factories
{
    public class WoodFactory : GoodFactory
    {
        public override Type GoodType => typeof(Wood);

        public override Wood CreateGood()
        {
            return new Wood();
        }
    }
}
