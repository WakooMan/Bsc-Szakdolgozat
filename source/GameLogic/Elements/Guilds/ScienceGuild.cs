using GameLogic.Elements.Effects;
using GameLogic.Elements.GameCards;

namespace GameLogic.Elements.Guilds
{
    public class ScienceGuild : Guild
    {
        public ScienceGuild()
        {
            m_victoryPoint = null;
        }

        public override Guild Clone()
        {
            return new ScienceGuild();
        }

        public override Task Apply(IGameContext gameContext, int playerId)
        {
            Player currentPlayer = gameContext.TurnHandler.CurrentPlayer;
            Player opponent = gameContext.TurnHandler.OpponentPlayer;
            currentPlayer.Money += GetMaxGreenCardCount(currentPlayer, opponent);
            
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
                Points = GetMaxGreenCardCount(owner, opponent)
            };
            return Task.CompletedTask;
        }

        private int GetMaxGreenCardCount(Player owner, Player opponent)
        {
            return Math.Max(owner.Cards.OfType<GreenCard>().Count(), opponent.Cards.OfType<GreenCard>().Count());
        }

        private VictoryPoints? m_victoryPoint;
    }
}