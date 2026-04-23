using SevenWonders.AI.Model.AIModelHandler;

namespace SevenWonders.AITrainerServer.Cache
{
    public class AIModelHandlerCache: IAIModelHandlerCache
    {
        public IAIModelHandler AIModelHandler => m_aIModelHandler;

        public AIModelHandlerCache(IAIDecisionHandlerCache aIDecisionHandlerCache, IPathProvider pathProvider)
        {
            m_aIModelHandler = new AIModelHandler(aIDecisionHandlerCache.TrainedAIModel, pathProvider);
        }

        private readonly IAIModelHandler m_aIModelHandler;
    }
}
