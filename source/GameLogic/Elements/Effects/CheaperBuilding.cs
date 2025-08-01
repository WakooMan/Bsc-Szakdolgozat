using GameLogic.Events.GameEvents;

namespace GameLogic.Elements.Effects
{
    public class CheaperBuilding : Effect
    {
        public int AmountOfResources { get; set; }
        public string BuildingType { get; set; }

        public CheaperBuilding() { }

        private CheaperBuilding(CheaperBuilding cheaperBuilding)
        {
            AmountOfResources = cheaperBuilding.AmountOfResources;
            BuildingType = cheaperBuilding.BuildingType;
        }

        public override CheaperBuilding Clone()
        {
            return new CheaperBuilding(this);
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
                eventArgs.CheaperBuildings.Add(this);
            }
        }
    }
}
