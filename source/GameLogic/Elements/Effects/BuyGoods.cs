using GameLogic.Events;

namespace GameLogic.Elements.Effects
{
    public class BuyGoods : Effect
    {
        public List<BuyGoodItem> BuyGoodItems { get; set; }

        public BuyGoods()
        {
            BuyGoodItems = new List<BuyGoodItem>();
        }

        private BuyGoods(BuyGoods buyGoods)
        {
            BuyGoodItems = buyGoods.BuyGoodItems.Select(b => new BuyGoodItem(b)).ToList();
        }

        public override BuyGoods Clone()
        {
            return new BuyGoods(this);
        }

        public override void Apply(IGameContext gameContext)
        {
            Player player = gameContext.TurnHandler.CurrentPlayer;
            gameContext.EventManager.Subscribe(GameEventType.BuildingCostCalculated, (args) => OnBuildingCostCalculated(player, args));
        }

        private void OnBuildingCostCalculated(Player player, EventArgs args)
        {
            if (args is OnBuildingCostCalculated eventArgs && player == eventArgs.Buyer)
            {
                eventArgs.BuyGoodItems.AddRange(BuyGoodItems);
            }
        }

    }
}
