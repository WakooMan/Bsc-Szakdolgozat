
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
            return Math.Max(playerProperties.Owner.Money, playerProperties.Opponent.Money) % 3;
        }
    }
}