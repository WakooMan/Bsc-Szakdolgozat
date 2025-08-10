namespace GameLogic.PlayerTurnStates
{
    public class EndTurn : IPlayerTurnState
    {
        public void ExecuteTurnState()
        {
            throw new NotImplementedException("This method is not implemented intentionally! Do not call it!");
        }

        public IPlayerTurnState GetNextTurnState()
        {
            return this;
        }
    }
}
