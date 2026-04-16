using GameLogic.Events.GameEvents;

namespace GameLogic.Elements.Effects
{
    public class Mathematics : Effect
    {
        public override Mathematics Clone()
        {
            return new Mathematics();
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
                Points = 3 * owner.Developments.Count
            };
            return Task.CompletedTask;
        }

        private VictoryPoints? m_victoryPoint;
    }
}
