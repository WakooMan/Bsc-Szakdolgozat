using SevenWonders.Web.Server.Contract.Messages.Game.ServerMessages;
using SevenWonders.Web.Server.Contract.Messages.Lobby.ServerMessages;

namespace SevenWonders.Web.Client.Model
{
    public interface IClientMessageDispatcher
    {
        void RegisterHandler(IMessageHandler messageHandler);
        void UnregisterHandler(IMessageHandler messageHandler);
        Task<bool> Dispatch(LobbyServerMessage message);
        Task<bool> Dispatch(GameServerMessage message);
    }
}
