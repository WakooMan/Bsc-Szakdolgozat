using WebServer.Contract.Messages.Game.ServerMessages;
using WebServer.Contract.Messages.Lobby.ServerMessages;

namespace SevenWonders.WebClient.Model
{
    public interface IMessageRegisterer
    {
        void Register<T>(LobbyResponseMessageHandlerDelegate<T> handler) where T : LobbyServerMessage;
        void Register<T>(GameResponseMessageHandlerDelegate<T> handler) where T : GameServerMessage;
        void Unregister<T>(LobbyResponseMessageHandlerDelegate<T> handler) where T : LobbyServerMessage;
        void Unregister<T>(GameResponseMessageHandlerDelegate<T> handler) where T : GameServerMessage;
    }
}
