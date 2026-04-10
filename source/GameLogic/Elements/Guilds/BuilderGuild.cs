
using GameLogic.Elements.Effects;
using GameLogic.Events.GameEvents;
using System;

namespace GameLogic.Elements.Guilds
{
    public class BuilderGuild : Guild
    {
        public BuilderGuild()
        {
            m_victoryPoints = new List<VictoryPoints>();
        }

        public override Guild Clone()
        {
            return new BuilderGuild();
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
            m_action = (eventObj) =>
            {
                Player currentPlayer = gameContext.TurnHandler.CurrentPlayer;
                Player opponent = gameContext.TurnHandler.OpponentPlayer;
                int maxCount = Math.Max(currentPlayer.Wonders.Where(wonder => wonder.HasBeenBuilt).Count(), opponent.Wonders.Where(wonder => wonder.HasBeenBuilt).Count());
                m_victoryPoint = new VictoryPoints()
                {
                    Points = maxCount * 2
                };
                m_victoryPoint.Apply(gameContext, playerId).GetAwaiter().GetResult();
            };
            gameContext.EventManager.Subscribe(m_action);
            return Task.CompletedTask;
        }

        private VictoryPoints? m_victoryPoint;
        private Action<BeforeGameEnded>? m_action;
    }
}
