
using GameLogic.Elements.Effects;
using GameLogic.Elements.Wonders;
using System.ComponentModel;

namespace GameLogic.Elements.Guilds
{
    public class ExtortionistGuild : Guild
    {
        public ExtortionistGuild()
        { }

        public override Guild Clone()
        {
            return new ExtortionistGuild();
        }

        public override async Task OnCalculatePlayerProperties(PlayerProperties playerProperties)
        {
            int maxCount = Math.Max(playerProperties.Owner.Money, playerProperties.Opponent.Money);

            VictoryPoints victoryPoints = new VictoryPoints()
            {
                Points = maxCount % 3
            };
            await victoryPoints.OnCalculatePlayerProperties(playerProperties);
        }
    }
}