using GameLogic.Elements;
using GameLogic.Interfaces;
using GameLogic.PlayerActions;

namespace SevenWondersUI
{
    public class PlayerActionReceiver : IPlayerActionReceiver
    {
        public IPlayerAction ReceivePlayerAction(Player player, ICollection<IPlayerAction> playerActions)
        {
            return null;
        }
    }
}
