using GameLogic.Interfaces;
using SevenWonders.WebClient.Model;

namespace SevenWonders.Presenter.PlayerActionReceivers
{
    public interface IRemotePlayerActionReceiver : IPlayerActionReceiver, IMessageHandler
    {
    }
}
