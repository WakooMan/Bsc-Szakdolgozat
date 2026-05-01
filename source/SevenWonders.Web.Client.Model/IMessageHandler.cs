using SevenWonders.Web.Server.Contract.Messages.Game.ServerMessages;
using SevenWonders.Web.Server.Contract.Messages.Lobby.ServerMessages;

namespace SevenWonders.Web.Client.Model
{
    public delegate Task<bool> LobbyResponseMessageHandlerDelegate<T>(T message) where T : LobbyServerMessage;
    public delegate Task<bool> GameResponseMessageHandlerDelegate<T>(T message) where T : GameServerMessage;

    public interface IMessageHandler
    {
        void Register(IMessageRegisterer registerer);

        void Unregister(IMessageRegisterer registerer);
    }
}
