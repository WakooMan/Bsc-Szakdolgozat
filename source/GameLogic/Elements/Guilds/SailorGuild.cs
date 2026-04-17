using GameLogic.Elements.Effects;
using GameLogic.Elements.GameCards;

namespace GameLogic.Elements.Guilds
{
    public class SailorGuild : Guild
    {
        public SailorGuild()
        {
            m_victoryPoint = null;
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
            if (m_victoryPoint is not null)
            {
                await m_victoryPoint.OnCalculatePlayerProperties(playerProperties);
            }
        }

        public override Task OnBeforeGameEnded(Player owner, Player opponent)
        {
            m_victoryPoint = new VictoryPoints()
            {
                Points = GetMaxCardCount(owner, opponent)
            };
            return Task.CompletedTask;
        }

        private int GetMaxCardCount(Player owner, Player opponent)
        {
            return Math.Max(owner.Cards.Where(c => c is GrayCard || c is BrownCard).Count(), opponent.Cards.Where(c => c is GrayCard || c is BrownCard).Count());
        }

        private VictoryPoints? m_victoryPoint;
    }
}