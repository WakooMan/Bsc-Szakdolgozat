using WebServer.Contract.Messages.Game.Requests;
using WebServer.Contract.Messages.Lobby;

namespace WebServer.Model.MessageHandling
{
    public interface IMessageRegisterer
    {
        void Register<T>(LobbyRequestMessageHandlerDelegate<T> handler) where T : LobbyRequestMessage;
        void Register<T>(GameRequestMessageHandlerDelegate<T> handler) where T : GameRequestMessage;
        void Unregister<T>(LobbyRequestMessageHandlerDelegate<T> handler) where T : LobbyRequestMessage;
        void Unregister<T>(GameRequestMessageHandlerDelegate<T> handler) where T : GameRequestMessage;
    }
}
