using GameLogic.Elements.Effects;
using GameLogic.Elements.GameCards;
using GameLogic.Elements.Wonders;
using System.ComponentModel;

namespace GameLogic.Elements.Guilds
{
    public class MagistrateGuild : Guild
    {
        public MagistrateGuild() { }

        public override Guild Clone()
        {
            return new MagistrateGuild();
        }

        public override Task Apply(IGameContext gameContext, Player owner, Player opponent)
        {
            owner.Money += GetMaxBlueCardCount(owner, opponent);
            return Task.CompletedTask;
        }

        public override async Task OnCalculatePlayerProperties(PlayerProperties playerProperties)
        {
            VictoryPoints victoryPoints = new VictoryPoints()
            {
                Points = GetMaxBlueCardCount(playerProperties.Owner, playerProperties.Opponent)
            };
            await victoryPoints.OnCalculatePlayerProperties(playerProperties);
        }

        private int GetMaxBlueCardCount(Player owner, Player opponent)
        {
            return Math.Max(owner.Cards.OfType<BlueCard>().Count(), opponent.Cards.OfType<BlueCard>().Count());
        }
    }
}