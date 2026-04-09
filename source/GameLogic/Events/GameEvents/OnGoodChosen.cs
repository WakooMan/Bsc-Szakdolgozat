using GameLogic.Elements.Goods.Factories;

namespace GameLogic.Events.GameEvents
{
    public class OnGoodChosen: GameEvent
    {
        public List<GoodFactory> GoodFactories { get; }
        public OnGoodChosen(ICollection<GoodFactory> goodFactories)
        {
            GoodFactories = goodFactories.ToList();
        }
    }
}
