
using GameLogic.Elements.Effects;
using GameLogic.Events.GameEvents;

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

        public override Task Apply(IGameContext gameContext, int playerId)
        {
            m_action = (eventObj) =>
            {
                Player currentPlayer = gameContext.TurnHandler.CurrentPlayer;
                Player opponent = gameContext.TurnHandler.OpponentPlayer;
                int maxCount = Math.Max(currentPlayer.Money, opponent.Money);

                m_victoryPoint = new VictoryPoints()
                {
                    Points = maxCount % 3
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