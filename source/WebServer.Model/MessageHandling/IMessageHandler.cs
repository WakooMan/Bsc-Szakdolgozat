using Microsoft.AspNetCore.SignalR;
using WebServer.Contract.Messages.Game.Requests;
using WebServer.Contract.Messages.Game.Responses;
using WebServer.Contract.Messages.Lobby;

namespace WebServer.Model.MessageHandling
{
    public delegate Task<LobbyResponseMessage> LobbyRequestMessageHandlerDelegate<T>(Hub hub, string connectionId, T message) where T : LobbyRequestMessage;
    public delegate Task<GameResponseMessage> GameRequestMessageHandlerDelegate<T>(Hub hub, string connectionId, T message) where T : GameRequestMessage;

    public interface IMessageHandler
    {
        void Register(IMessageRegisterer registerer);

        void Unregister(IMessageRegisterer registerer);
    }
}
