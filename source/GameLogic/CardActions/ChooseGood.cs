using GameLogic.Goods.Factories;

namespace GameLogic.CardActions
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
