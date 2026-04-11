using Microsoft.AspNetCore.SignalR;
using WebServer.Contract.Messages.Game.Requests;
using WebServer.Contract.Messages.Game.Responses;
using WebServer.Contract.Messages.Lobby;

namespace WebServer.Model.MessageHandling
{
    public interface IServerMessageDispatcher
    {
        void RegisterHandler(IMessageHandler messageHandler);
        void UnregisterHandler(IMessageHandler messageHandler);
        Task<LobbyResponseMessage> Dispatch(Hub hub, string connectionId, LobbyRequestMessage message);
        Task<GameResponseMessage> Dispatch(Hub hub, string connectionId, GameRequestMessage message);
    }
}
