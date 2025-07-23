using GameLogic.Events;

namespace GameLogic.PlayerTurnStates
{
    public interface IPlayerTurnState
    {
        void ExecuteTurnState(IEventManager eventManager);
        IPlayerTurnState GetNextTurnState();
    }
}
