using GameLogic.Interfaces;
using SevenWonders.Common;

namespace SevenWonders.Presenter.PlayerActionReceivers
{
    public interface IPlayerActionReceiverFactory
    {
        IPlayerActionReceiver Create(PlayerType playerType, string playerName);
    }
}
