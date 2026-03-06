using GameLogic.PlayerActions;
using SevenWonders.Presenter.PlayerActionHandler;

namespace SevenWonders.Presenter.PlayerActionWaiters
{
    public class PickCardActionWaiter : IPlayerActionWaiter<PickCard>
    {
        public PickCardActionWaiter() { }
        public PickCard WaitForPlayerAction(ICollection<PickCard> playerActions)
        {
            while (true)
            {
                Thread.Sleep(100);
            }
        }
    }
}
