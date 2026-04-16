
using GameLogic.Elements.Effects;

namespace GameLogic.Elements.Guilds
{
    public class BuilderGuild : Guild
    {
        public BuilderGuild() { }

        private BuilderGuild(VictoryPoints? victoryPoint)
        {
            m_victoryPoint = victoryPoint;
        }

        public override Guild Clone()
        {
            return new BuilderGuild(m_victoryPoint?.Clone());
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
            int maxCount = Math.Max(owner.Wonders.Where(wonder => wonder.HasBeenBuilt).Count(), opponent.Wonders.Where(wonder => wonder.HasBeenBuilt).Count());
            m_victoryPoint = new VictoryPoints()
            {
                Points = maxCount * 2
            };
            return Task.CompletedTask;
        }

        private VictoryPoints? m_victoryPoint;
    }
}
