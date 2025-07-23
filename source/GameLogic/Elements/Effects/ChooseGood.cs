using GameLogic.Elements.Goods.Factories;
using GameLogic.Events;
using GameLogic.GameStates;

namespace GameLogic.Elements.Effects
{
    public class ChooseGood : Effect
    {
        public List<GoodFactory> GoodFactories { get; set; }

        public ChooseGood()
        {
            GoodFactories = new List<GoodFactory>();
        }

        public ChooseGood(ChooseGood chooseGood)
        {
            GoodFactories = chooseGood.GoodFactories;
        }

        public override ChooseGood Clone()
        {
            return new ChooseGood(this);
        }

        public override void Apply(PlayingState game)
        {
            game.EventManager.Subscribe(GameEventType.BuildingCostCalculated, OnBuildingCostCalculated);
        }

        private void OnBuildingCostCalculated(EventArgs eventArgs)
        {
            if (eventArgs is OnBuildingCostCalculated cardEventArgs)
            {
                cardEventArgs.AdditionalGoods.Add(this);
            }
        }
    }
}
