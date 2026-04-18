using GameLogic.Elements.Goods;
using GameLogic.Elements.Goods.Factories;

namespace GameLogic.Elements.Effects
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
