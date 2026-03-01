using GameLogic.Elements;
using GameLogic.Interfaces;
using GameLogic.PlayerActions;

namespace SevenWonders.Presenter.PlayerActionReceivers
{
    public class PlayerActionReceiver : IPlayerActionReceiver
    {
        public IPlayerAction ReceivePlayerAction(Player player, ICollection<IPlayerAction> playerActions)
        {
            throw new NotImplementedException();
        }
    }
}
