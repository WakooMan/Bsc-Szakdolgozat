using SevenWonders.Game.Logic.Elements.Goods;
using SevenWonders.Game.Logic.Elements.Goods.Resources;

namespace SevenWonders.Game.Logic.Elements.Goods.Factories
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
