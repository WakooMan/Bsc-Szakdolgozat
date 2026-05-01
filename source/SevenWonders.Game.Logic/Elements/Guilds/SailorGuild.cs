using SevenWonders.Game.Logic.Elements.Effects;
using SevenWonders.Game.Logic.Elements.GameCards;
using SevenWonders.Game.Logic.Elements.Wonders;
using System.ComponentModel;

namespace SevenWonders.Game.Logic.Elements.Guilds
{
    public class SailorGuild : Guild
    {
        public SailorGuild()
        {
        }

        public override Guild Clone()
        {
            return new SailorGuild();
        }

        public override void Apply(IGameContext gameContext, Player owner, Player opponent)
        {
            owner.Money += GetMaxCardCount(owner, opponent);
        }

        public override void OnCalculatePlayerProperties(PlayerProperties playerProperties)
        {
            VictoryPoints victoryPoints = new VictoryPoints()
            {
                Points = CalculateGuildVP(playerProperties)
            };
            victoryPoints.OnCalculatePlayerProperties(playerProperties);
        }

        private int GetMaxCardCount(Player owner, Player opponent)
        {
            return Math.Max(owner.Cards.Where(c => c is GrayCard || c is BrownCard).Count(), opponent.Cards.Where(c => c is GrayCard || c is BrownCard).Count());
        }

        public override int CalculateGuildVP(PlayerProperties playerProperties)
        {
            return GetMaxCardCount(playerProperties.Owner, playerProperties.Opponent);
        }

        public override int CalculateMoney(PlayerProperties playerProperties)
        {
            return GetMaxCardCount(playerProperties.Owner, playerProperties.Opponent);
        }
    }
}