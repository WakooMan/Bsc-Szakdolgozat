using GameLogic.Elements.Effects;
using GameLogic.Elements.GameCards;
using GameLogic.Elements.Wonders;
using System.ComponentModel;

namespace GameLogic.Elements.Guilds
{
    public class StrategistGuild : Guild
    {
        public StrategistGuild()
        {
        }

        public override Guild Clone()
        {
            return new StrategistGuild();
        }

        public override void Apply(IGameContext gameContext, Player owner, Player opponent)
        {
            owner.Money += GetMaxRedCardCount(owner, opponent);
        }

        public override void OnCalculatePlayerProperties(PlayerProperties playerProperties)
        {
            VictoryPoints victoryPoints = new VictoryPoints()
            {
                Points = CalculateGuildVP(playerProperties)
            };
            victoryPoints.OnCalculatePlayerProperties(playerProperties);
        }

        private int GetMaxRedCardCount(Player owner, Player opponent)
        {
            return Math.Max(owner.Cards.OfType<RedCard>().Count(), opponent.Cards.OfType<RedCard>().Count());
        }

        public override int CalculateGuildVP(PlayerProperties playerProperties)
        {
            return GetMaxRedCardCount(playerProperties.Owner, playerProperties.Opponent);
        }

        public override int CalculateMoney(PlayerProperties playerProperties)
        {
            return GetMaxRedCardCount(playerProperties.Owner, playerProperties.Opponent);
        }
    }
}