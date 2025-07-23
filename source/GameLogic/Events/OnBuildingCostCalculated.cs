using GameLogic.Elements;
using GameLogic.Elements.Effects;
using GameLogic.GameStructures;

namespace GameLogic.Events
{
    public class OnBuildingCostCalculated: EventArgs
    {
        public Player Player { get; set; }

        public List<ChooseGood> AdditionalGoods { get; set; }

        public OnBuildingCostCalculated(Player player)
        {
            Player = player;
            AdditionalGoods = new List<ChooseGood>();
        }
    }
}
