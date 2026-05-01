using SevenWonders.Game.Logic.Elements.Goods;
using SevenWonders.Game.Logic.Elements.Goods.Factories;

namespace SevenWonders.Game.Logic.Elements.Effects
{
    public class ChooseGood : Effect
    {
        public List<GoodFactory> GoodFactories { get; set; }

        public ChooseGood()
        {
            GoodFactories = new List<GoodFactory>();
        }

        public IReadOnlyList<Good> GetGoods()
        {
            return GoodFactories.Select(factory => factory.CreateGood()).ToList();
        }

        public override ChooseGood Clone()
        {
            return new ChooseGood(this);
        }

        private ChooseGood(ChooseGood chooseGood)
        {
            GoodFactories = chooseGood.GoodFactories;
        }
    }
}
