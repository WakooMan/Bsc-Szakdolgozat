namespace GameLogic.GameStates
{
    public class EndGameState : IGameState
    {
        public void DoStateAction() { }

        public IGameState GetNextState()
        {
            return new EndGameState();
        }
    }
}
