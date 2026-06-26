namespace SevenWondersUI.Services.ConnectionStates
{
    public class ConnectedState : IConnectionState
    {
        public ConnectedState(IConnectionContext connectionContext)
        {
            m_connectionContext = connectionContext;
        }
        public Task<bool> Execute()
        {
            return Task.FromResult(true);
        }

        public IConnectionState NextState()
        {
            return m_connectionContext.ConnectedState;
        }

        public IConnectionState PreviousState()
        {
            return m_connectionContext.ReceiveLobbiesState;
        }

        public Task Undo()
        {
            return Task.CompletedTask;
        }

        private readonly IConnectionContext m_connectionContext;
    }
}
