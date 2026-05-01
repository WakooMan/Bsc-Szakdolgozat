using SevenWonders.Web.Server.Contract.Messages.Game.ClientMessages;
using SevenWonders.Web.Server.Contract.Messages.Lobby.ClientMessages;

namespace SevenWonders.Web.Server.Model.MessageHandling
{
    public interface IMessageRegisterer
    {
        void Register<T>(LobbyRequestMessageHandlerDelegate<T> handler) where T : LobbyClientMessage;
        void Register<T>(GameRequestMessageHandlerDelegate<T> handler) where T : GameClientMessage;
        void Unregister<T>(LobbyRequestMessageHandlerDelegate<T> handler) where T : LobbyClientMessage;
        void Unregister<T>(GameRequestMessageHandlerDelegate<T> handler) where T : GameClientMessage;
    }
}
