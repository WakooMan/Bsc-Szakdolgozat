using SevenWonders.Web.Server.Contract.Messages.Game.ClientMessages;
using SevenWonders.Web.Server.Contract.Messages.Lobby.ClientMessages;

namespace SevenWonders.Web.Server.Model.MessageHandling
{
    public interface IServerMessageDispatcher
    {
        void RegisterHandler(IMessageHandler messageHandler);
        void UnregisterHandler(IMessageHandler messageHandler);
        Task Dispatch(string connectionId, LobbyClientMessage message);
        Task Dispatch(string connectionId, GameClientMessage message);
    }
}
