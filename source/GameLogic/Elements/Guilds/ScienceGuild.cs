using GameLogic.Elements.Effects;
using GameLogic.Elements.GameCards;
using GameLogic.Elements.Wonders;
using System.ComponentModel;

namespace GameLogic.Elements.Guilds
{
    public class ScienceGuild : Guild
    {
        public ScienceGuild()
        {
        }

        public override Guild Clone()
        {
            return new ScienceGuild();
        }

        public override void Apply(IGameContext gameContext, Player owner, Player opponent)
        {
            owner.Money += GetMaxGreenCardCount(owner, opponent);
        }

        public override void OnCalculatePlayerProperties(PlayerProperties playerProperties)
        {
            VictoryPoints victoryPoints = new VictoryPoints()
            {
                Points = CalculateGuildVP(playerProperties)
            };
            victoryPoints.OnCalculatePlayerProperties(playerProperties);
        }

        private int GetMaxGreenCardCount(Player owner, Player opponent)
        {
            return Math.Max(owner.Cards.OfType<GreenCard>().Count(), opponent.Cards.OfType<GreenCard>().Count());
        }

        public override int CalculateGuildVP(PlayerProperties playerProperties)
        {
            return GetMaxGreenCardCount(playerProperties.Owner, playerProperties.Opponent);
        }

        public override int CalculateMoney(PlayerProperties playerProperties)
        {
            return GetMaxGreenCardCount(playerProperties.Owner, playerProperties.Opponent);
        }
    }
}