using GameLogic.Events.GameEvents;

namespace GameLogic.Elements.Effects
{
    public class Mathematics : Effect
    {
        public override Mathematics Clone()
        {
            return new Mathematics();
        }

        public override Task Apply(IGameContext gameContext, int playerId)
        {
            Player currentPlayer = gameContext.TurnHandler.CurrentPlayer;
            Player opponent = gameContext.TurnHandler.OpponentPlayer;
            m_action = (eventObj) =>
            {
                int maxCount = currentPlayer.Developments.Count;
                m_victoryPoint = new VictoryPoints()
                {
                    Points = 3 * currentPlayer.Developments.Count
                };
                m_victoryPoint.Apply(gameContext, playerId).GetAwaiter().GetResult();
            };
            gameContext.EventManager.Subscribe(m_action);
            return Task.CompletedTask;
        }

        public override async Task Unapply(IGameContext gameContext, int playerId)
        {
            if (m_action is not null)
            {
                gameContext.EventManager.Unsubscribe(m_action);
            }
            if (m_victoryPoint is not null)
            {
                await m_victoryPoint.Unapply(gameContext, playerId);
                m_victoryPoint = null;
            }
        }

        private Action<BeforeGameEnded>? m_action;
        private VictoryPoints? m_victoryPoint;
    }
}
