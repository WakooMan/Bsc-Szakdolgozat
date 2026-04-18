using GameLogic.Elements.Effects;
using GameLogic.Elements.GameCards;
using GameLogic.Elements.Wonders;
using System.ComponentModel;

namespace GameLogic.Elements.Guilds
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

        public override Task Apply(IGameContext gameContext, Player owner, Player opponent)
        {
            owner.Money += GetMaxCardCount(owner, opponent);
            return Task.CompletedTask;
        }

        public override async Task OnCalculatePlayerProperties(PlayerProperties playerProperties)
        {
            VictoryPoints victoryPoints = new VictoryPoints()
            {
                Points = GetMaxCardCount(playerProperties.Owner, playerProperties.Opponent)
            };
            await victoryPoints.OnCalculatePlayerProperties(playerProperties);
        }

        private int GetMaxCardCount(Player owner, Player opponent)
        {
            return Math.Max(owner.Cards.Where(c => c is GrayCard || c is BrownCard).Count(), opponent.Cards.Where(c => c is GrayCard || c is BrownCard).Count());
        }
    }
}