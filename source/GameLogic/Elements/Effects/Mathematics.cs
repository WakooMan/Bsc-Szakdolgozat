namespace GameLogic.Elements.Effects
{
    public class Mathematics : Effect
    {
        public override Mathematics Clone()
        {
            return new Mathematics();
        }

        public override async Task OnCalculatePlayerProperties(PlayerProperties playerProperties)
        {
            VictoryPoints victoryPoints = new VictoryPoints()
            {
                Points = 3 * playerProperties.Owner.Developments.Count
            };
            await victoryPoints.OnCalculatePlayerProperties(playerProperties);
        }
    }
}
