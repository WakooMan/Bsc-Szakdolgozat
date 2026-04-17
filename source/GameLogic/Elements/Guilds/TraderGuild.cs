
using GameLogic.Elements.Effects;
using GameLogic.Elements.GameCards;

namespace GameLogic.Elements.Guilds
{
    public class TraderGuild : Guild
    {
        public TraderGuild()
        {
            m_victoryPoint = null;
        }

        public override Guild Clone()
        {
            return new TraderGuild();
        }

        public override Task Apply(IGameContext gameContext, Player owner, Player opponent)
        {
            owner.Money += GetMaxYellowCardCount(owner, opponent);
            return Task.CompletedTask;
        }

        public override async Task OnCalculatePlayerProperties(PlayerProperties playerProperties)
        {
            if (m_victoryPoint is not null)
            {
                await m_victoryPoint.OnCalculatePlayerProperties(playerProperties);
            }
        }

        public override Task OnBeforeGameEnded(Player owner, Player opponent)
        {
            m_victoryPoint = new VictoryPoints()
            {
                Points = GetMaxYellowCardCount(owner, opponent)
            };
            return Task.CompletedTask;
        }

        private int GetMaxYellowCardCount(Player owner, Player opponent)
        {
            return Math.Max(owner.Cards.OfType<YellowCard>().Count(), opponent.Cards.OfType<YellowCard>().Count());
        }

        private VictoryPoints? m_victoryPoint;
    }
}
