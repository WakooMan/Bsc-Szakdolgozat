using GameLogic.Interfaces;
using SevenWonders.WebClient.Model;
using SevenWonders.WebClient.Model.Services;

namespace SevenWonders.Presenter.PlayerActionReceivers
{
    public interface ILocalPlayerActionReceiver: IPlayerActionReceiver, IMessageHandler
    {
        IClientHubService? ClientHubService { get; set; }
    }
}
