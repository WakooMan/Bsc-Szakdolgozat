using SevenWonders.Game.Logic.Events;

namespace SevenWonders.Game.Logic.PlayerTurnStates
{
    public interface IPlayerTurnState
    {
        void ExecuteTurnState();
        IPlayerTurnState GetNextTurnState();
    }
}
