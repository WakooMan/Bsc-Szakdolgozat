using SevenWonders.AI.Model.DecisionRouter.DecisionHandlers;

namespace SevenWonders.AI.Model.Cache
{
    public interface IAIDecisionHandlerCache
    {
        IAIDecisionHandler MediumAI { get; }
        IAIDecisionHandler EasyAI { get; }
    }
}
