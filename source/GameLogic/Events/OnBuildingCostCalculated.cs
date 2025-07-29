using GameLogic.Elements;
using GameLogic.Elements.Effects;

namespace GameLogic.Events
{
    public class OnBuildingCostCalculated: EventArgs
    {
        public List<BuyGoodItem> BuyGoodItems { get; }
        public List<CheaperBuilding> CheaperBuildings { get; }
        public Player Buyer { get; }

        public OnBuildingCostCalculated(Player buyer)
        {
            Buyer = buyer;
            BuyGoodItems = new List<BuyGoodItem>();
            CheaperBuildings = new List<CheaperBuilding>();
        }
    }
}
