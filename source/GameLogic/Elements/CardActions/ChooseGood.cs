using GameLogic.Elements.Goods.Factories;

namespace GameLogic.Elements.CardActions
{
    public class ChooseGood : CardAction
    {
        public List<GoodFactory> GoodFactories { get; set; }

        public ChooseGood()
        {
            GoodFactories = new List<GoodFactory>();
        }
    }
}
