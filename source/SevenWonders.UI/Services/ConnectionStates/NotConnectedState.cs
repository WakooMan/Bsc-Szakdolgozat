namespace SevenWondersUI.Services.ConnectionStates
{
    public class NotConnectedState : IConnectionState
    {
        public NotConnectedState(IConnectionContext connectionContext)
        {
            m_connectionContext = connectionContext;
        }
        public Task<bool> Execute()
        {
            return Task.FromResult(true);
        }

        public Task Undo()
        {
            return Task.CompletedTask;
        }

        public IConnectionState NextState()
        {
            return m_connectionContext.LoginState;
        }

        public IConnectionState PreviousState()
        {
            return m_connectionContext.NotConnectedState;
        }

        private readonly IConnectionContext m_connectionContext;
    }
}
