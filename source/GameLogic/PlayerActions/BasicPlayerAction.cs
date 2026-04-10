namespace GameLogic.PlayerActions
{
    public class BasicPlayerAction : IPlayerAction
    {
        public string Name { get; }
        public bool Completed { get; }
        public BasicPlayerAction(string name, bool completed)
        {
            Name = name;
            Completed = completed;
        }
        public Task<bool> DoPlayerAction(IGameContext gameContext)
        {
            return Task.FromResult(Completed);
        }
        public Task<bool> CanPerform(IGameContext gameContext)
        {
            return Task.FromResult(true);
        }
    }
}
