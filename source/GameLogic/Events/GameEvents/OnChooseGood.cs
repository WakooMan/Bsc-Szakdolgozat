using GameLogic.Elements.Goods.Factories;

namespace GameLogic.Events.GameEvents
{
    public class OnChooseGood : GameEvent
    {
        public List<GoodFactory> GoodFactories { get; }
        public OnChooseGood(ICollection<GoodFactory> goodFactories)
        {
            GoodFactories = goodFactories.ToList();
        }
    }
}
