namespace SevenWonders.GameEngine
{
    public class GameEngineTicker : IGameEngineTicker
    {
        public GameEngineTicker(IDispatcher dispatcher)
        {
            m_dispatcherTimer = dispatcher.CreateTimer();
        }

        public event EventHandler? Tick;

        public TimeSpan Interval { get => m_dispatcherTimer.Interval; set => m_dispatcherTimer.Interval = value; }

        public void Start()
        {
            m_dispatcherTimer.Tick += OnTick;
            m_dispatcherTimer.Start();
        }

        public void Stop()
        {
            m_dispatcherTimer.Stop();
        }

        private void OnTick(object? sender, EventArgs e)
        {
            Tick?.Invoke(sender, e);
        }

        private readonly IDispatcherTimer m_dispatcherTimer;
    }
}
