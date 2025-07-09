using GameLogic.Elements.Goods;
using GameLogic.Elements.Goods.Resources;

namespace GameLogic.Elements.Goods.Factories
{
    public class StoneFactory : GoodFactory
    {
        public override Good CreateGood()
        {
            return new Stone();
        }
    }
}
