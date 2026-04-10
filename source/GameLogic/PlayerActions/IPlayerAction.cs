using GameLogic.Elements;

namespace GameLogic.PlayerActions
{
    public interface IPlayerAction
    {
        string Name { get; }

        Task<bool> DoPlayerAction(IGameContext gameContext);

        Task<bool> CanPerform(IGameContext gameContext);
    }
}
