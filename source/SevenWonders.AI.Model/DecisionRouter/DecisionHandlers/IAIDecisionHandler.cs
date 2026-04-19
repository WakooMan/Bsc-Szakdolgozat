using SevenWonders.AI.Model.DecisionRouter;
using SevenWonders.AI.Model.Messages;

namespace SevenWonders.AI.Model.DecisionRouter.DecisionHandlers
{
    public interface IAIDecisionHandler : IDecisionHandler
    {
        void Initialize();
        void Uninitialize();
        void Decide(ActionRequest actionRequest);
        event Action<GameStateResponse> OnGameStateReceived;
    }
}
