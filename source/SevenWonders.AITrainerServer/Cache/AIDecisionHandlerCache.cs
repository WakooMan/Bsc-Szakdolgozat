using GameLogic;
using SevenWonders.AI.Model.DecisionRouter.DecisionHandlers;
using SevenWonders.AITrainerServer.Factories;

namespace SevenWonders.AITrainerServer.Cache
{
    public class AIDecisionHandlerCache : IAIDecisionHandlerCache
    {
        public IAIDecisionHandler TrainingAI => m_trainingAI;
        public IAIDecisionHandler TrainedAIModel => m_easyAI;

        public AIDecisionHandlerCache(IGame game, 
                                      IGameStateVectorReceiverFactory gameStateVectorReceiverFactory, 
                                      IPlayerActionMaskReceiverFactory playerActionMaskReceiverFactory,
                                      IRewardCalculatorFactory rewardCalculatorFactory)
        {
            m_trainingAI = new AIDecisionHandler(game, gameStateVectorReceiverFactory.CreateMedium(), playerActionMaskReceiverFactory.Create(), rewardCalculatorFactory.Create());
            m_easyAI = new AIDecisionHandler(game, gameStateVectorReceiverFactory.CreateEasy(), playerActionMaskReceiverFactory.Create(), rewardCalculatorFactory.Create());
        }

        private readonly IAIDecisionHandler m_trainingAI;
        private readonly IAIDecisionHandler m_easyAI;
    }
}
