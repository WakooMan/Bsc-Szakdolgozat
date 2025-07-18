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

        public override void Apply()
        {
            throw new NotImplementedException();
        }

        public override CheaperBuilding Clone()
        {
            return new CheaperBuilding(this);
        }
    }
}
