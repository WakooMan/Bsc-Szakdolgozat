using SevenWonders.AI.Model.Messages;

namespace SevenWonders.AI.Model.DecisionRouter.DecisionHandlers
{
    public interface IAIDecisionHandler : IDecisionHandler
    {
        void Initialize();
        void Uninitialize();
        Func<GameStateResponse, ActionRequest>? OnGameStateReceived { get; set; }
    }
}
