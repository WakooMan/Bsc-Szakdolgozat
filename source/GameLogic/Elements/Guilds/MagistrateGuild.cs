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

        public override void Apply(IGameContext gameContext, Player owner, Player opponent)
        {
            owner.Money += GetMaxBlueCardCount(owner, opponent);
        }

        public override void OnCalculatePlayerProperties(PlayerProperties playerProperties)
        {
            VictoryPoints victoryPoints = new VictoryPoints()
            {
                Points = CalculateGuildVP(playerProperties)
            };
            victoryPoints.OnCalculatePlayerProperties(playerProperties);
        }

        private int GetMaxBlueCardCount(Player owner, Player opponent)
        {
            return Math.Max(owner.Cards.OfType<BlueCard>().Count(), opponent.Cards.OfType<BlueCard>().Count());
        }

        override public int CalculateGuildVP(PlayerProperties playerProperties)
        {
            return GetMaxBlueCardCount(playerProperties.Owner, playerProperties.Opponent);
        }

        public override int CalculateMoney(PlayerProperties playerProperties)
        {
            return GetMaxBlueCardCount(playerProperties.Owner, playerProperties.Opponent);
        }
    }
}