using GameLogic.Events;

namespace GameLogic.PlayerTurnStates
{
    public interface IPlayerTurnState
    {
        void ExecuteTurnState();
        IPlayerTurnState GetNextTurnState();
    }
}
