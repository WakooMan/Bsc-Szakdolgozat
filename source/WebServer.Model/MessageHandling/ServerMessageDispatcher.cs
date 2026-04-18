using WebServer.Contract.Messages.Game.ClientMessages;
using WebServer.Contract.Messages.Lobby.ClientMessages;
using WebServer.Model.MessageHandling.Factories;

namespace WebServer.Model.MessageHandling
{
    public class ServerMessageDispatcher: IServerMessageDispatcher, IDisposable
    {
        public ServerMessageDispatcher(IMessageRegistererFactory messageRegistererFactory, ILobbyMessageHandlers lobbyMessageHandlers)
        {
            m_gameRequestHandlers = new Dictionary<Type, List<object>>();
            m_lobbyRequestHandlers = new Dictionary<Type, List<object>>();
            m_registeredHandlers = new List<IMessageHandler>();
            m_messageRegisterer = messageRegistererFactory.Create(m_lobbyRequestHandlers, m_gameRequestHandlers);
            m_lobbyMessageHandlers = lobbyMessageHandlers;
            RegisterHandler(m_lobbyMessageHandlers);
        }
        public void RegisterHandler(IMessageHandler messageHandler)
        {
            if (m_registeredHandlers.Contains(messageHandler))
                return;
            messageHandler.Register(m_messageRegisterer);
            m_registeredHandlers.Add(messageHandler);
        }

        public void UnregisterHandler(IMessageHandler messageHandler)
        {
            if (!m_registeredHandlers.Contains(messageHandler))
                return;
            messageHandler.Unregister(m_messageRegisterer);
            m_registeredHandlers.Remove(messageHandler);
        }

        public async Task Dispatch(string connectionId, LobbyClientMessage message)
        {
            if (!m_lobbyRequestHandlers.ContainsKey(message.GetType()))
                return;
            foreach (var obj in m_lobbyRequestHandlers[message.GetType()])
            {
                object? result = ((Delegate)obj)?.DynamicInvoke(new object[] { connectionId, message });
                if (result is not null)
                {
                    await (Task)result;
                }
            }
        }

        public async Task Dispatch(string connectionId, GameClientMessage message)
        {
            if (!m_gameRequestHandlers.ContainsKey(message.GetType()))
                return;
            foreach (var obj in m_gameRequestHandlers[message.GetType()])
            {
                object? result = ((Delegate)obj)?.DynamicInvoke(new object[] { connectionId, message });
                if (result is not null)
                {
                    await (Task)result;
                }
            }
        }

        public void Dispose()
        {
            UnregisterHandler(m_lobbyMessageHandlers);
        }

        private readonly Dictionary<Type, List<object>> m_lobbyRequestHandlers = new Dictionary<Type, List<object>>();
        private readonly Dictionary<Type, List<object>> m_gameRequestHandlers = new Dictionary<Type, List<object>>();
        private readonly IMessageRegisterer m_messageRegisterer;
        private readonly ILobbyMessageHandlers m_lobbyMessageHandlers;
        private readonly List<IMessageHandler> m_registeredHandlers;
    }
}
