using GameLogic.Goods.Resources;

namespace GameLogic.Goods.Factories
{
    public class ClayFactory : GoodFactory
    {
        public override Good CreateGood()
        {
            return new Clay();
        }
    }
}
