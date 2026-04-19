namespace GameLogic.PlayerActions
{
    public class BasicPlayerAction : IPlayerAction
    {
        public string Name { get; }
        public bool Completed { get; }

        public int Id => 3;

        public BasicPlayerAction(string name, bool completed)
        {
            Name = name;
            Completed = completed;
        }
        public bool DoPlayerAction(IGameContext gameContext)
        {
            return Completed;
        }
        public bool CanPerform(IGameContext gameContext)
        {
            return true;
        }
    }
}
