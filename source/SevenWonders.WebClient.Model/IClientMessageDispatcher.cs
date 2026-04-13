using WebServer.Contract.Messages.Game.ServerMessages;
using WebServer.Contract.Messages.Lobby.ServerMessages;

namespace SevenWonders.WebClient.Model
{
    public interface IClientMessageDispatcher
    {
        void RegisterHandler(IMessageHandler messageHandler);
        void UnregisterHandler(IMessageHandler messageHandler);
        Task<bool> Dispatch(LobbyServerMessage message);
        Task<bool> Dispatch(GameServerMessage message);
    }
}
