using Microsoft.AspNetCore.SignalR;
using SevenWonders.Web.Server.Contract.Messages.Game.ClientMessages;
using SevenWonders.Web.Server.Contract.Messages.Lobby.ClientMessages;

namespace SevenWonders.Web.Server.Model.MessageHandling
{
    public delegate Task LobbyRequestMessageHandlerDelegate<T>(string connectionId, T message) where T : LobbyClientMessage;
    public delegate Task GameRequestMessageHandlerDelegate<T>(string connectionId, T message) where T : GameClientMessage;

    public interface IMessageHandler
    {
        void Register(IMessageRegisterer registerer);

        void Unregister(IMessageRegisterer registerer);
    }
}
