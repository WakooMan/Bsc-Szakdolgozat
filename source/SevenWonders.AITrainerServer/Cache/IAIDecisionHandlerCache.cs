using SevenWonders.AI.Model.DecisionRouter.DecisionHandlers;

namespace SevenWonders.AITrainerServer.Cache
{
    public interface IAIDecisionHandlerCache
    {
        IAIDecisionHandler TrainingAI { get; }
        IAIDecisionHandler TrainedAIModel { get; }
    }
}
