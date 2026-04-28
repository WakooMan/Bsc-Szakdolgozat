
using GameLogic.Elements.Effects;
using GameLogic.Elements.GameCards;
using GameLogic.Elements.Wonders;
using System.ComponentModel;

namespace GameLogic.Elements.Guilds
{
    public class TraderGuild : Guild
    {
        public TraderGuild()
        {
        }

        public override Guild Clone()
        {
            return new TraderGuild();
        }

        public override void Apply(IGameContext gameContext, Player owner, Player opponent)
        {
            owner.Money += GetMaxYellowCardCount(owner, opponent);
        }

        public override void OnCalculatePlayerProperties(PlayerProperties playerProperties)
        {
            VictoryPoints victoryPoints = new VictoryPoints()
            {
                Points = CalculateGuildVP(playerProperties)
            };
            victoryPoints.OnCalculatePlayerProperties(playerProperties);
        }

        private int GetMaxYellowCardCount(Player owner, Player opponent)
        {
            return Math.Max(owner.Cards.OfType<YellowCard>().Count(), opponent.Cards.OfType<YellowCard>().Count());
        }

        public override int CalculateGuildVP(PlayerProperties playerProperties)
        {
            return GetMaxYellowCardCount(playerProperties.Owner, playerProperties.Opponent);
        }

        public override int CalculateMoney(PlayerProperties playerProperties)
        {
            return GetMaxYellowCardCount(playerProperties.Owner, playerProperties.Opponent);
        }
    }
}
