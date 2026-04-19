using SevenWonders.AI.Model.DecisionRouter;

namespace SevenWonders.AI.Model.DecisionRouter.Factories
{
    public interface IDecisionRouterFactory
    {
        IDecisionRouter Create(IDecisionHandler pyramidDecisionHandler);
    }
}
