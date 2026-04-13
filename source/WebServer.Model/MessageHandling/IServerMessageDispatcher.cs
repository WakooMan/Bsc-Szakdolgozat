using Microsoft.AspNetCore.SignalR;
using WebServer.Contract.Messages.Game.ClientMessages;
using WebServer.Contract.Messages.Game.ServerMessages;
using WebServer.Contract.Messages.Lobby.ClientMessages;
using WebServer.Contract.Messages.Lobby.ServerMessages;

namespace WebServer.Model.MessageHandling
{
    public interface IServerMessageDispatcher
    {
        void RegisterHandler(IMessageHandler messageHandler);
        void UnregisterHandler(IMessageHandler messageHandler);
        Task<LobbyServerMessage> Dispatch(Hub hub, string connectionId, LobbyClientMessage message);
        Task<GameServerMessage> Dispatch(Hub hub, string connectionId, GameClientMessage message);
    }
}
