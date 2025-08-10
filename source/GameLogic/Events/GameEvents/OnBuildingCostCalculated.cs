using GameLogic.Elements;
using GameLogic.Elements.Effects;

namespace GameLogic.Events.GameEvents
{
    public class OnBuildingCostCalculated: GameEvent
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
