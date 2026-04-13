using Microsoft.AspNetCore.SignalR;
using WebServer.Contract.Messages.Game.ClientMessages;
using WebServer.Contract.Messages.Game.ServerMessages;
using WebServer.Contract.Messages.Lobby.ClientMessages;
using WebServer.Contract.Messages.Lobby.ServerMessages;

namespace WebServer.Model.MessageHandling
{
    public delegate Task<LobbyServerMessage> LobbyRequestMessageHandlerDelegate<T>(Hub hub, string connectionId, T message) where T : LobbyClientMessage;
    public delegate Task<GameServerMessage> GameRequestMessageHandlerDelegate<T>(Hub hub, string connectionId, T message) where T : GameClientMessage;

    public interface IMessageHandler
    {
        void Register(IMessageRegisterer registerer);

        void Unregister(IMessageRegisterer registerer);
    }
}
