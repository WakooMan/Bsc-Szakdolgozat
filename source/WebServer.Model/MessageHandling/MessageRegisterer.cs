using WebServer.Contract.Messages.Game.Requests;
using WebServer.Contract.Messages.Lobby;

namespace WebServer.Model.MessageHandling
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

        public void Register<T>(LobbyRequestMessageHandlerDelegate<T> handler) where T : LobbyRequestMessage
        {
            if (m_lobbyRequestHandlers.ContainsKey(typeof(T)))
            {
                throw new InvalidOperationException($"Lobby request message handler cannot be added for type {nameof(T)}!");
            }
            m_lobbyRequestHandlers[typeof(T)] = handler;
        }

        public void Register<T>(GameRequestMessageHandlerDelegate<T> handler) where T : GameRequestMessage
        {
            if (m_gameRequestHandlers.ContainsKey(typeof(T)))
            {
                throw new InvalidOperationException($"Game request message handler cannot be added for type {nameof(T)}!");
            }
            m_gameRequestHandlers[typeof(T)] = handler;
        }

        public void Unregister<T>(LobbyRequestMessageHandlerDelegate<T> handler) where T : LobbyRequestMessage
        {
            if (!m_lobbyRequestHandlers.ContainsKey(typeof(T)) || (LobbyRequestMessageHandlerDelegate<T>)m_lobbyRequestHandlers[typeof(T)] != handler)
                return;
            m_lobbyRequestHandlers.Remove(typeof(T));
        }

        public void Unregister<T>(GameRequestMessageHandlerDelegate<T> handler) where T : GameRequestMessage
        {
            if (!m_gameRequestHandlers.ContainsKey(typeof(T)) || (GameRequestMessageHandlerDelegate<T>)m_gameRequestHandlers[typeof(T)] != handler)
                return;
            m_gameRequestHandlers.Remove(typeof(T));
        }
    }
}
