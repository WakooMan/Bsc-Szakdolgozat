using SevenWonders.Game.Logic.Elements.Goods;
using SevenWonders.Game.Logic.Elements.Goods.Resources;

namespace SevenWonders.Game.Logic.Elements.Goods.Factories
{
    public class ClayFactory : GoodFactory
    {
        public override Type GoodType => typeof(Clay);

        public override Clay CreateGood()
        {
            return new Clay();
        }
    }
}
