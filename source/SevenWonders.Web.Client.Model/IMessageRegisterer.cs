using SevenWonders.Web.Server.Contract.Messages.Game.ServerMessages;
using SevenWonders.Web.Server.Contract.Messages.Lobby.ServerMessages;

namespace SevenWonders.Web.Client.Model
{
    public interface IMessageRegisterer
    {
        void Register<T>(LobbyResponseMessageHandlerDelegate<T> handler) where T : LobbyServerMessage;
        void Register<T>(GameResponseMessageHandlerDelegate<T> handler) where T : GameServerMessage;
        void Unregister<T>(LobbyResponseMessageHandlerDelegate<T> handler) where T : LobbyServerMessage;
        void Unregister<T>(GameResponseMessageHandlerDelegate<T> handler) where T : GameServerMessage;
    }
}
