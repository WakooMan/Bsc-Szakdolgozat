namespace SevenWonders.Game.Engine
{
    public interface IGameEngineTicker
    {
        TimeSpan Interval { get; set; }
        event EventHandler Tick;
        void Start();
        void Stop();
    }
}
