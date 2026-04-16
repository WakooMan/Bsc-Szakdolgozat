
using GameLogic.Elements.Effects;

namespace GameLogic.Elements.Guilds
{
    public class ExtortionistGuild : Guild
    {
        public ExtortionistGuild()
        {
            m_victoryPoint = null;
        }

        public override Guild Clone()
        {
            return new ExtortionistGuild();
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
            int maxCount = Math.Max(owner.Money, opponent.Money);

            m_victoryPoint = new VictoryPoints()
            {
                Points = maxCount % 3
            };

            return Task.CompletedTask;
        }

        private VictoryPoints? m_victoryPoint;
    }
}