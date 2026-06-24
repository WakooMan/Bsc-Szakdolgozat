using SevenWonders.Game.Logic.Elements;
using SevenWonders.Game.Logic.Interfaces;
using SevenWonders.AI.Model.DecisionRouter;
using SevenWonders.AI.Model.DecisionRouter.Factories;
using SevenWonders.Common;
using SevenWonders.Game.Logic.Exceptions;

namespace SevenWonders.AI.Model.PlayerActionReceivers
{
    public class NonPlayerActionReceiver: IPlayerActionReceiver
    {
        public NonPlayerActionReceiver(IDecisionRouterFactory decisionRouterFactory, IDecisionHandler pyramidDecisionHandler)
        {
            m_decisionRouter = decisionRouterFactory.Create(pyramidDecisionHandler);
            m_isEnded = false;
        }

        public PlayerActionWrapper ReceivePlayerAction(Player player, ICollection<PlayerActionWrapper> playerActions)
        {
            if (m_isEnded)
            {
                throw new EndGameException();
            }

            GameLog.Info($"ReceivePlayerAction: Player={player.Name}, ActionCount={playerActions.Count}");
            var result = m_decisionRouter.RoutePlayerAction(player, playerActions);
            GameLog.Info($"Selected action: {result.PlayerAction.GetType().Name}");
            return result;
        }

        public void EndGame()
        {
            m_isEnded = true;
        }

        private readonly IDecisionRouter m_decisionRouter;
        private bool m_isEnded;
    }
}
