using GameLogic.Elements;
using GameLogic.PlayerActions;

namespace GameLogic.Handlers
{
    public interface IPlayerActionHandler
    {
        Task HandlePlayerActionsCompleted(IGameContext gameContext, Player player, ICollection<IPlayerAction> playerActions);
        Task<bool> HandlePlayerActions(IGameContext gameContext, Player player, ICollection<IPlayerAction> playerActions);

        Task HandlePlayerAction(IGameContext gameContext, Player player, IPlayerAction playerAction);
    }
}
