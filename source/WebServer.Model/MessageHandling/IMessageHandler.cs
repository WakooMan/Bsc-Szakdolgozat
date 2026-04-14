using Microsoft.AspNetCore.SignalR;
using WebServer.Contract.Messages.Game.ClientMessages;
using WebServer.Contract.Messages.Lobby.ClientMessages;

namespace WebServer.Model.MessageHandling
{
    public delegate Task LobbyRequestMessageHandlerDelegate<T>(string connectionId, T message) where T : LobbyClientMessage;
    public delegate Task GameRequestMessageHandlerDelegate<T>(string connectionId, T message) where T : GameClientMessage;

    public interface IMessageHandler
    {
        void Register(IMessageRegisterer registerer);

        void Unregister(IMessageRegisterer registerer);
    }
}
