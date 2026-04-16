using GameLogic.Elements.Effects;
using GameLogic.Elements.GameCards;

namespace GameLogic.Elements.Guilds
{
    public class StrategistGuild : Guild
    {
        public StrategistGuild()
        {
            m_victoryPoint = null;
        }

        public override Guild Clone()
        {
            return new StrategistGuild();
        }

        public override Task Apply(IGameContext gameContext, int playerId)
        {
            Player currentPlayer = gameContext.TurnHandler.CurrentPlayer;
            Player opponent = gameContext.TurnHandler.OpponentPlayer;
            currentPlayer.Money += GetMaxRedCardCount(currentPlayer, opponent);
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
                Points = GetMaxRedCardCount(owner, opponent)
            };
            return Task.CompletedTask;
        }

        private int GetMaxRedCardCount(Player owner, Player opponent)
        {
            return Math.Max(owner.Cards.OfType<RedCard>().Count(), opponent.Cards.OfType<RedCard>().Count());
        }

        private VictoryPoints? m_victoryPoint;
    }
}