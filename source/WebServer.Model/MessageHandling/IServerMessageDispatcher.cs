using WebServer.Contract.Messages.Game.ClientMessages;
using WebServer.Contract.Messages.Lobby.ClientMessages;

namespace WebServer.Model.MessageHandling
{
    public interface IServerMessageDispatcher
    {
        void RegisterHandler(IMessageHandler messageHandler);
        void UnregisterHandler(IMessageHandler messageHandler);
        Task Dispatch(string connectionId, LobbyClientMessage message);
        Task Dispatch(string connectionId, GameClientMessage message);
    }
}
