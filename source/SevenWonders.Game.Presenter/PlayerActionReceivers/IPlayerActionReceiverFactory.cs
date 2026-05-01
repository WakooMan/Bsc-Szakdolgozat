using SevenWonders.Game.Logic.Interfaces;
using SevenWonders.Common;

namespace SevenWonders.Game.Presenter.PlayerActionReceivers
{
    public interface IPlayerActionReceiverFactory
    {
        IPlayerActionReceiver Create(PlayerType playerType, string playerName);
    }
}
