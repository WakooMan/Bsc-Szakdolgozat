namespace SevenWonders.Game.Logic.Elements.Effects
{
    public class Mathematics : Effect
    {
        public override Mathematics Clone()
        {
            return new Mathematics();
        }

        public override void OnCalculatePlayerProperties(PlayerProperties playerProperties)
        {
            VictoryPoints victoryPoints = new VictoryPoints()
            {
                Points = 3 * playerProperties.Owner.Developments.Count
            };
            victoryPoints.OnCalculatePlayerProperties(playerProperties);
        }
    }
}
