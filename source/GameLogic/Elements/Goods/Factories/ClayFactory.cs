using GameLogic.Elements.Goods;
using GameLogic.Elements.Goods.Resources;

namespace GameLogic.Elements.Goods.Factories
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
