using SevenWonders.Web.Server.Contract.Messages.Game.ServerMessages;
using SevenWonders.Web.Server.Contract.Messages.Lobby.ServerMessages;

namespace SevenWonders.Web.Client.Model
{
    public class MessageRegisterer: IMessageRegisterer
    {
        private readonly Dictionary<Type, object> m_lobbyRequestHandlers;
        private readonly Dictionary<Type, object> m_gameRequestHandlers;
        public MessageRegisterer(Dictionary<Type, object> lobbyRequestHandlers, Dictionary<Type, object> gameRequestHandlers)
        {
            m_lobbyRequestHandlers = lobbyRequestHandlers;
            m_gameRequestHandlers = gameRequestHandlers;
        }

        public void Register<T>(LobbyResponseMessageHandlerDelegate<T> handler) where T : LobbyServerMessage
        {
            if (m_lobbyRequestHandlers.ContainsKey(typeof(T)))
            {
                throw new InvalidOperationException($"Lobby request message handler cannot be added for type {nameof(T)}!");
            }
            m_lobbyRequestHandlers[typeof(T)] = handler;
        }

        public void Register<T>(GameResponseMessageHandlerDelegate<T> handler) where T : GameServerMessage
        {
            if (m_gameRequestHandlers.ContainsKey(typeof(T)))
            {
                throw new InvalidOperationException($"Game request message handler cannot be added for type {nameof(T)}!");
            }
            m_gameRequestHandlers[typeof(T)] = handler;
        }

        public void Unregister<T>(LobbyResponseMessageHandlerDelegate<T> handler) where T : LobbyServerMessage
        {
            if (!m_lobbyRequestHandlers.ContainsKey(typeof(T)) || (LobbyResponseMessageHandlerDelegate<T>)m_lobbyRequestHandlers[typeof(T)] != handler)
                return;
            m_lobbyRequestHandlers.Remove(typeof(T));
        }

        public void Unregister<T>(GameResponseMessageHandlerDelegate<T> handler) where T : GameServerMessage
        {
            if (!m_gameRequestHandlers.ContainsKey(typeof(T)) || (GameResponseMessageHandlerDelegate<T>)m_gameRequestHandlers[typeof(T)] != handler)
                return;
            m_gameRequestHandlers.Remove(typeof(T));
        }
    }
}
