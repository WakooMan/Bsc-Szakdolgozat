
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

        public override async Task OnCalculatePlayerProperties(PlayerProperties playerProperties)
        {
            int maxCount = Math.Max(playerProperties.Owner.Wonders.Where(wonder => wonder.HasBeenBuilt).Count(), playerProperties.Opponent.Wonders.Where(wonder => wonder.HasBeenBuilt).Count());
            VictoryPoints victoryPoints = new VictoryPoints()
            {
                Points = maxCount * 2
            };
            await victoryPoints.OnCalculatePlayerProperties(playerProperties);
        }
    }
}
