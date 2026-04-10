using GameLogic.Events;

namespace GameLogic.PlayerTurnStates
{
    public interface IPlayerTurnState
    {
        Task ExecuteTurnState();
        IPlayerTurnState GetNextTurnState();
    }
}
