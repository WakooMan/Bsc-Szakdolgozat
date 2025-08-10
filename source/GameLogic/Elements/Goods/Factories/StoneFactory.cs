using GameLogic.Elements.Goods;
using GameLogic.Elements.Goods.Resources;

namespace GameLogic.Elements.Goods.Factories
{
    public class StoneFactory : GoodFactory
    {
        public override Type GoodType => typeof(Stone);

        public override Stone CreateGood()
        {
            return new Stone();
        }
    }
}
