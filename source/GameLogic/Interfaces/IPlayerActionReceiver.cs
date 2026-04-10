using GameLogic.Elements;

namespace GameLogic.Interfaces
{
    public interface IPlayerActionReceiver
    {
        PlayerActionWrapper ReceivePlayerAction(Player player, ICollection<PlayerActionWrapper> playerActions);
    }
}
