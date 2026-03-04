using GameLogic.Elements;
using GameLogic.PlayerActions;

namespace GameLogic.Interfaces
{
    public interface IPlayerActionReceiver
    {
        TPlayerAction ReceivePlayerAction<TPlayerAction>(Player player, ICollection<TPlayerAction> playerActions) where TPlayerAction : class, IPlayerAction, new();
    }
}
