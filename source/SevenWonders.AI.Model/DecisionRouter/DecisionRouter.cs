using GameLogic.Elements;
using GameLogic.Interfaces;
using GameLogic.PlayerActions;
using SevenWonders.AI.Model.DecisionRouter.CommonDecisionHandlers;
using SevenWonders.Common;

namespace SevenWonders.AI.Model.DecisionRouter
{
    public class DecisionRouter : IDecisionRouter
    {
        public DecisionRouter(IWeightConfiguration weightConfiguration, IDecisionHandler pyramidDecisionHandler)
        {
            m_pyramidDecisionHandler = pyramidDecisionHandler;
            m_handlers = new Dictionary<Type, IDecisionHandler>
            {
                { typeof(ChooseDevelopmentAction), new ChooseDevelopmentHandler(weightConfiguration) },
                { typeof(DropCard), new ChooseRemoveCardHandler(weightConfiguration) },
                { typeof(ChooseDroppedCardAction), new ChooseDroppedCardHandler(weightConfiguration) },
                { typeof(BuildWonder), new BuildWonderHandler(weightConfiguration) },
                { typeof(ChooseWonderAction), new ChooseWonderDecisionHandler(weightConfiguration) }
            };
        }
        public PlayerActionWrapper RoutePlayerAction(Player player, ICollection<PlayerActionWrapper> playerActions)
        {

            Type? type = null;
            foreach (var actionWrapper in playerActions)
            {
                Type actionType = actionWrapper.PlayerAction.GetType();
                if (m_handlers.ContainsKey(actionType))
                {
                    type = actionType;
                    break;
                }
            }

            GameLog.Info($"RoutePlayerAction: Player={player.Name}, ActionCount={playerActions.Count}, ActionType={type?.Name ?? "null"}");

            if (type is not null && m_handlers.TryGetValue(type, out IDecisionHandler? handler))
            {
                GameLog.Info($"Using specialized handler: {handler.GetType().Name}");
                return handler.HandleDecisions(player, playerActions);
            }
            else
            {
                GameLog.Info($"Using pyramid handler: {m_pyramidDecisionHandler.GetType().Name}");
                return m_pyramidDecisionHandler.HandleDecisions(player, playerActions);
            }
        }

        private readonly IDictionary<Type, IDecisionHandler> m_handlers;
        private readonly IDecisionHandler m_pyramidDecisionHandler;
    }
}
