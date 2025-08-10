using GameLogic.Elements.Goods;
using GameLogic.Elements.Goods.Resources;

namespace GameLogic.Elements.Goods.Factories
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
