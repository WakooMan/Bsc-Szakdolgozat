namespace GameLogic.PlayerActions
{
    public interface IPlayerAction
    {
        string Name { get; }
        int Id { get; }

        bool DoPlayerAction(IGameContext gameContext);

        bool CanPerform(IGameContext gameContext);
    }
}
