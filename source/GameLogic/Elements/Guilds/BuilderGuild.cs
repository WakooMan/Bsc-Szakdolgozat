
using GameLogic.Elements.Effects;

namespace GameLogic.Elements.Guilds
{
    public class BuilderGuild : Guild
    {
        public BuilderGuild() { }

        public override Guild Clone()
        {
            return new BuilderGuild();
        }

        public override void OnCalculatePlayerProperties(PlayerProperties playerProperties)
        {
            VictoryPoints victoryPoints = new VictoryPoints()
            {
                Points = CalculateGuildVP(playerProperties)
            };
            victoryPoints.OnCalculatePlayerProperties(playerProperties);
        }

        public override int CalculateGuildVP(PlayerProperties playerProperties)
        {
            return Math.Max(playerProperties.Owner.Wonders.Where(wonder => wonder.HasBeenBuilt).Count(), playerProperties.Opponent.Wonders.Where(wonder => wonder.HasBeenBuilt).Count()) * 2;
        }
    }
}
