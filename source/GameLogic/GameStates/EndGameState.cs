namespace GameLogic.GameStates
{
    public class EndGameState : IGameState
    {
        public Task DoStateAction()
        {
            throw new NotImplementedException("This method intentionally throws this exception, do not call this method on this object!");
        }

        public IGameState GetNextState()
        {
            return this;
        }
    }
}
