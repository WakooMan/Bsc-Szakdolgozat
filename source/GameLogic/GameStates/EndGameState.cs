namespace GameLogic.GameStates
{
    public class EndGameState : IGameState
    {
        public void DoStateAction()
        {
            throw new NotImplementedException("This method intentionally throws this exception, do not call this method on this object!");
        }

        public IGameState GetNextState()
        {
            return this;
        }
    }
}
