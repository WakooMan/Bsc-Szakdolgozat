using GameLogic.Goods.Resources;

namespace GameLogic.Goods.Factories
{
    public class WoodFactory : GoodFactory
    {
        public override Good CreateGood()
        {
            return new Wood();
        }
    }
}
