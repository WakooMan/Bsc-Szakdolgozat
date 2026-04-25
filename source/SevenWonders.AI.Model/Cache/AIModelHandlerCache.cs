using SevenWonders.AI.Model.AIModelHandler;

namespace SevenWonders.AI.Model.Cache
{
    public class AIModelHandlerCache: IAIModelHandlerCache
    {
        public IAIModelHandler EasyAIModelHandler => m_easyAIModelHandler;
        public IAIModelHandler MediumAIModelHandler => m_mediumAIModelHandler;

        public AIModelHandlerCache(IAIDecisionHandlerCache aIDecisionHandlerCache, IPathProvider pathProvider)
        {
            m_easyAIModelHandler = new AIModelHandler.AIModelHandler(aIDecisionHandlerCache.EasyAI, pathProvider);
            m_mediumAIModelHandler = new AIModelHandler.AIModelHandler(aIDecisionHandlerCache.MediumAI, pathProvider);
        }

        private readonly IAIModelHandler m_easyAIModelHandler;
        private readonly IAIModelHandler m_mediumAIModelHandler;
    }
}
