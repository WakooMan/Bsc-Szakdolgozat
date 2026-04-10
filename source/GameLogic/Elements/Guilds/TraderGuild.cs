
using GameLogic.Elements.Effects;
using GameLogic.Elements.GameCards;
using GameLogic.Events.GameEvents;
using System;

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

        public override Task Apply(IGameContext gameContext, int playerId)
        {
            Player currentPlayer = gameContext.TurnHandler.CurrentPlayer;
            Player opponent = gameContext.TurnHandler.OpponentPlayer;
            int maxCount = Math.Max(currentPlayer.Cards.OfType<YellowCard>().Count(), opponent.Cards.OfType<YellowCard>().Count());
            currentPlayer.Money += maxCount;

            m_action = (eventObj) =>
            {
                Player currentPlayer = gameContext.TurnHandler.CurrentPlayer;
                Player opponent = gameContext.TurnHandler.OpponentPlayer;
                int maxCount = Math.Max(currentPlayer.Cards.OfType<YellowCard>().Count(), opponent.Cards.OfType<YellowCard>().Count());
                m_victoryPoint = new VictoryPoints()
                {
                    Points = maxCount
                };
                m_victoryPoint.Apply(gameContext, playerId).GetAwaiter().GetResult();
            };
            gameContext.EventManager.Subscribe(m_action);
            return Task.CompletedTask;
        }

        private Action<BeforeGameEnded>? m_action;
        private VictoryPoints? m_victoryPoint;
    }
}
