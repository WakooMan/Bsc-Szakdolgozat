using GameLogic.Elements;
using GameLogic.PlayerActions;

namespace GameLogic.Handlers
{
    public interface IPlayerActionHandler
    {
        IPlayerAction? HandlePlayerActionsCompleted(IGameContext gameContext, Player player, ICollection<IPlayerAction> playerActions);
        (bool completed, IPlayerAction? playerAction) HandlePlayerActions(IGameContext gameContext, Player player, ICollection<IPlayerAction> playerActions);

        bool HandlePlayerAction(IGameContext gameContext, Player player, IPlayerAction playerAction);
    }
}
