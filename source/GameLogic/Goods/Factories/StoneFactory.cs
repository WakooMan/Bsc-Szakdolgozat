using GameLogic.Goods.Resources;

namespace GameLogic.Goods.Factories
{
    public class StoneFactory : GoodFactory
    {
        public override Good CreateGood()
        {
            return new Stone();
        }
    }
}
