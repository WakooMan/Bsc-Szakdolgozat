using GameLogic.Elements.Effects;
using GameLogic.Elements.GameCards;

namespace GameLogic.Elements.Guilds
{
    public class MagistrateGuild : Guild
    {
        public MagistrateGuild()
        {
            m_victoryPoint = null;
        }

        public override Guild Clone()
        {
            return new MagistrateGuild();
        }

        public override Task Apply(IGameContext gameContext, int playerId)
        {
            Player currentPlayer = gameContext.TurnHandler.CurrentPlayer;
            Player opponent = gameContext.TurnHandler.OpponentPlayer;

            // TODO: Apply should get the two players
            currentPlayer.Money += GetMaxBlueCardCount(currentPlayer, opponent);
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
                Points = GetMaxBlueCardCount(owner, opponent)
            };
            return Task.CompletedTask;
        }

        private int GetMaxBlueCardCount(Player owner, Player opponent)
        {
            return Math.Max(owner.Cards.OfType<BlueCard>().Count(), opponent.Cards.OfType<BlueCard>().Count());
        }

        private VictoryPoints? m_victoryPoint;
    }
}