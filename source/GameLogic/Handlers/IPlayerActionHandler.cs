using GameLogic.Elements;
using GameLogic.PlayerActions;

namespace GameLogic.Handlers
{
    public interface IPlayerActionHandler
    {
        Task<IPlayerAction?> HandlePlayerActionsCompleted(IGameContext gameContext, Player player, ICollection<IPlayerAction> playerActions);
        Task<(bool completed, IPlayerAction? playerAction)> HandlePlayerActions(IGameContext gameContext, Player player, ICollection<IPlayerAction> playerActions);

        Task<bool> HandlePlayerAction(IGameContext gameContext, Player player, IPlayerAction playerAction);
    }
}
