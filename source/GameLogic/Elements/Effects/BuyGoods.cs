using GameLogic.Events.GameEvents;

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
            gameContext.EventManager.Subscribe<OnBuildingCostCalculated>((args) => OnBuildingCostCalculated(player, args));
        }

        private void OnBuildingCostCalculated(Player player, OnBuildingCostCalculated eventArgs)
        {
            if (player == eventArgs.Buyer)
            {
                eventArgs.BuyGoodItems.AddRange(BuyGoodItems);
            }
        }

    }
}
