using GameLogic.Events;

namespace GameLogic.PlayerTurnStates
{
    public class EndTurn : IPlayerTurnState
    {
        public void ExecuteTurnState(IEventManager eventManager)
        {
        }

        public IPlayerTurnState GetNextTurnState()
        {
            return null;
        }
    }
}
