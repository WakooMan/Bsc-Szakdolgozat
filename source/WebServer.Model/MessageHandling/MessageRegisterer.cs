using WebServer.Contract.Messages.Game.ClientMessages;
using WebServer.Contract.Messages.Lobby.ClientMessages;

namespace WebServer.Model.MessageHandling
{
    public class MessageRegisterer: IMessageRegisterer
    {
        private readonly Dictionary<Type, List<object>> m_lobbyRequestHandlers;
        private readonly Dictionary<Type, List<object>> m_gameRequestHandlers;
        public MessageRegisterer(Dictionary<Type, List<object>> lobbyRequestHandlers, Dictionary<Type, List<object>> gameRequestHandlers)
        {
            m_lobbyRequestHandlers = lobbyRequestHandlers;
            m_gameRequestHandlers = gameRequestHandlers;
        }

        public void Register<T>(LobbyRequestMessageHandlerDelegate<T> handler) where T : LobbyClientMessage
        {
            if (m_lobbyRequestHandlers.ContainsKey(typeof(T)) && !m_lobbyRequestHandlers[typeof(T)].Contains(handler))
            {
                m_lobbyRequestHandlers[typeof(T)].Add(handler);
            }
            else
            {
                m_lobbyRequestHandlers[typeof(T)] = [handler];
            }
        }

        public void Register<T>(GameRequestMessageHandlerDelegate<T> handler) where T : GameClientMessage
        {
            if (m_gameRequestHandlers.ContainsKey(typeof(T)) && !m_gameRequestHandlers[typeof(T)].Contains(handler))
            {
                m_gameRequestHandlers[typeof(T)].Add(handler);
            }
            else
            {
                m_gameRequestHandlers[typeof(T)] = [handler];
            }
        }

        public void Unregister<T>(LobbyRequestMessageHandlerDelegate<T> handler) where T : LobbyClientMessage
        {
            if (!m_lobbyRequestHandlers.ContainsKey(typeof(T)) || !m_lobbyRequestHandlers[typeof(T)].Contains(handler))
                return;
            m_lobbyRequestHandlers[typeof(T)].Remove(handler);
            if (m_lobbyRequestHandlers[typeof(T)].Count == 0)
            {
                m_lobbyRequestHandlers.Remove(typeof(T));
            }
        }

        public void Unregister<T>(GameRequestMessageHandlerDelegate<T> handler) where T : GameClientMessage
        {
            if (!m_gameRequestHandlers.ContainsKey(typeof(T)) || !m_gameRequestHandlers[typeof(T)].Contains(handler))
                return;
            m_gameRequestHandlers[typeof(T)].Remove(handler);
            if (m_gameRequestHandlers[typeof(T)].Count == 0)
            {
                m_gameRequestHandlers.Remove(typeof(T));
            }
        }
    }
}
