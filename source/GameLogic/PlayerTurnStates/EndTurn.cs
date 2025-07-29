using GameLogic.Events;

namespace GameLogic.PlayerTurnStates
{
    public class EndTurn : IPlayerTurnState
    {
        public void ExecuteTurnState()
        {
        }

        public IPlayerTurnState GetNextTurnState()
        {
            return null;
        }
    }
}
