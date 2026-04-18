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

        public override Task Apply(IGameContext gameContext, Player owner, Player opponent)
        {
            owner.Money += GetMaxGreenCardCount(owner, opponent);
            return Task.CompletedTask;
        }

        public override async Task OnCalculatePlayerProperties(PlayerProperties playerProperties)
        {
            VictoryPoints victoryPoints = new VictoryPoints()
            {
                Points = GetMaxGreenCardCount(playerProperties.Owner, playerProperties.Opponent)
            };
            await victoryPoints.OnCalculatePlayerProperties(playerProperties);
        }

        private int GetMaxGreenCardCount(Player owner, Player opponent)
        {
            return Math.Max(owner.Cards.OfType<GreenCard>().Count(), opponent.Cards.OfType<GreenCard>().Count());
        }
    }
}