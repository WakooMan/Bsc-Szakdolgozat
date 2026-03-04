using GameLogic.PlayerActions;

namespace SevenWonders.Presenter.PlayerActionHandler
{
    public interface IPlayerActionWaiter<TPlayerAction> where TPlayerAction : class, IPlayerAction, new()
    {
        TPlayerAction WaitForPlayerAction(ICollection<TPlayerAction> playerActions);
    }
}
