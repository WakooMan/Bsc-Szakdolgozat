using SevenWonders.Game.Logic.Elements;
using SevenWonders.Game.Logic.Interfaces;
using SevenWonders.AI.Model.DecisionRouter;
using SevenWonders.AI.Model.DecisionRouter.Factories;
using SevenWonders.Common;

namespace SevenWonders.AI.Model.PlayerActionReceivers
{
    public class NonPlayerActionReceiver: IPlayerActionReceiver
    {
        public NonPlayerActionReceiver(IDecisionRouterFactory decisionRouterFactory, IDecisionHandler pyramidDecisionHandler)
        {
            m_decisionRouter = decisionRouterFactory.Create(pyramidDecisionHandler);
        }

        public PlayerActionWrapper ReceivePlayerAction(Player player, ICollection<PlayerActionWrapper> playerActions)
        {
            GameLog.Info($"ReceivePlayerAction: Player={player.Name}, ActionCount={playerActions.Count}");
            var result = m_decisionRouter.RoutePlayerAction(player, playerActions);
            GameLog.Info($"Selected action: {result.PlayerAction.GetType().Name}");
            return result;
        }

        private readonly IDecisionRouter m_decisionRouter;
    }
}
