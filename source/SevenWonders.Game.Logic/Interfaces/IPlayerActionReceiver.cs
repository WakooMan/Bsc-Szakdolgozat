using SevenWonders.Game.Logic.Elements;

namespace SevenWonders.Game.Logic.Interfaces
{
    public interface IPlayerActionReceiver
    {
        void EndGame();
        PlayerActionWrapper ReceivePlayerAction(Player player, ICollection<PlayerActionWrapper> playerActions);
    }
}
