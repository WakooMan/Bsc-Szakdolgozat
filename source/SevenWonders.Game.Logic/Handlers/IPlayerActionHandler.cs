using SevenWonders.Game.Logic.Elements;
using SevenWonders.Game.Logic.PlayerActions;

namespace SevenWonders.Game.Logic.Handlers
{
    public interface IPlayerActionHandler
    {
        IPlayerAction? HandlePlayerActionsCompleted(IGameContext gameContext, Player player, ICollection<IPlayerAction> playerActions);
        (bool completed, IPlayerAction? playerAction) HandlePlayerActions(IGameContext gameContext, Player player, ICollection<IPlayerAction> playerActions);

        bool HandlePlayerAction(IGameContext gameContext, Player player, IPlayerAction playerAction);
    }
}
