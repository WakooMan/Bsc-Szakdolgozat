namespace SevenWondersUI.Services.ConnectionStates
{
    public interface IConnectionState
    {
        Task<bool> Execute();
        Task Undo();
        IConnectionState NextState();
        IConnectionState PreviousState();
    }
}
