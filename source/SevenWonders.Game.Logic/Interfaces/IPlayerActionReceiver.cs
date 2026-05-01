using SevenWonders.Game.Logic.Elements;

namespace SevenWonders.Game.Logic.Interfaces
{
    public interface IPlayerActionReceiver
    {
        PlayerActionWrapper ReceivePlayerAction(Player player, ICollection<PlayerActionWrapper> playerActions);
    }
}
