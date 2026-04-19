namespace SevenWonders.AI.Model.DecisionRouter.Factories
{
    public class DecisionRouterFactory : IDecisionRouterFactory
    {
        public DecisionRouterFactory(IWeightConfiguration weightConfiguration)
        {
            m_weightConfiguration = weightConfiguration;
        }

        public IDecisionRouter Create(IDecisionHandler pyramidDecisionHandler)
        {
            return new DecisionRouter(m_weightConfiguration, pyramidDecisionHandler);
        }

        private readonly IWeightConfiguration m_weightConfiguration;
    }
}
