using SevenWonders.Game.Logic.Interfaces;
using SevenWonders.Web.Client.Model;
using SevenWonders.Web.Client.Model.Services;

namespace SevenWonders.Game.Presenter.PlayerActionReceivers
{
    public interface ILocalPlayerActionReceiver: IPlayerActionReceiver, IMessageHandler
    {
        IClientHubService? ClientHubService { get; set; }
    }
}
