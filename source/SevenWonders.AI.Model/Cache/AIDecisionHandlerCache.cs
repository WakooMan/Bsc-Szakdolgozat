using GameLogic;
using SevenWonders.AI.Model.DecisionRouter.DecisionHandlers;
using SevenWonders.AI.Model.Factories;

namespace SevenWonders.AI.Model.Cache
{
    public class AIDecisionHandlerCache : IAIDecisionHandlerCache
    {
        public IAIDecisionHandler MediumAI => m_mediumAI;
        public IAIDecisionHandler EasyAI => m_easyAI;

        public AIDecisionHandlerCache(IGame game, 
                                      IGameStateVectorReceiverFactory gameStateVectorReceiverFactory, 
                                      IPlayerActionMaskReceiverFactory playerActionMaskReceiverFactory,
                                      IRewardCalculatorFactory rewardCalculatorFactory)
        {
            m_mediumAI = new AIDecisionHandler(game, gameStateVectorReceiverFactory.CreateMedium(), playerActionMaskReceiverFactory.Create(), rewardCalculatorFactory.Create());
            m_easyAI = new AIDecisionHandler(game, gameStateVectorReceiverFactory.CreateEasy(), playerActionMaskReceiverFactory.Create(), rewardCalculatorFactory.Create());
        }

        private readonly IAIDecisionHandler m_mediumAI;
        private readonly IAIDecisionHandler m_easyAI;
    }
}
