namespace SevenWonders.Game.Logic.GameStates
{
    public interface IGameState
    {
        void DoStateAction();
        IGameState GetNextState();
    }
}
