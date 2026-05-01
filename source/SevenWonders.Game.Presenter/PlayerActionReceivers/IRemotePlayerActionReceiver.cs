using SevenWonders.Game.Logic.Interfaces;
using SevenWonders.Web.Client.Model;

namespace SevenWonders.Game.Presenter.PlayerActionReceivers
{
    public interface IRemotePlayerActionReceiver : IPlayerActionReceiver, IMessageHandler
    {
    }
}
