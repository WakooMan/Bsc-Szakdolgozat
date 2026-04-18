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

        public override Task Apply(IGameContext gameContext, Player owner, Player opponent)
        {
            owner.Money += GetMaxRedCardCount(owner, opponent);
            return Task.CompletedTask;
        }

        public override async Task OnCalculatePlayerProperties(PlayerProperties playerProperties)
        {
            VictoryPoints victoryPoints = new VictoryPoints()
            {
                Points = GetMaxRedCardCount(playerProperties.Owner, playerProperties.Opponent)
            };
            await victoryPoints.OnCalculatePlayerProperties(playerProperties);
        }

        private int GetMaxRedCardCount(Player owner, Player opponent)
        {
            return Math.Max(owner.Cards.OfType<RedCard>().Count(), opponent.Cards.OfType<RedCard>().Count());
        }
    }
}