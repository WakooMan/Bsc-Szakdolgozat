using GameLogic.Elements;
using GameLogic.PlayerActions;

namespace GameLogic.Interfaces
{
    public interface IPlayerActionReceiver
    {
        IPlayerAction ReceivePlayerAction(Player player, ICollection<IPlayerAction> playerActions);

        T ReceivePlayerAction<T>(Player player, ICollection<T> playerActions) where T: IPlayerAction;
    }
}
