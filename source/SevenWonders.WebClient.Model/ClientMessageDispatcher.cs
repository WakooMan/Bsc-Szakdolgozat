using SevenWonders.WebClient.Model.Factories;
using WebServer.Contract.Messages.Game.ServerMessages;
using WebServer.Contract.Messages.Lobby.ServerMessages;

namespace SevenWonders.WebClient.Model
{
    public class ClientMessageDispatcher: IClientMessageDispatcher
    {
        public ClientMessageDispatcher(IMessageRegistererFactory messageRegistererFactory)
        {
            m_gameResponseHandlers = new Dictionary<Type, object>();
            m_lobbyResponseHandlers = new Dictionary<Type, object>();
            m_registeredHandlers = new Dictionary<Type, IMessageHandler>();
            m_messageRegisterer = messageRegistererFactory.Create(m_lobbyResponseHandlers, m_gameResponseHandlers);
        }
        public void RegisterHandler(IMessageHandler messageHandler)
        {
            Type messageHandlerType = messageHandler.GetType();
            if (m_registeredHandlers.ContainsKey(messageHandlerType))
                return;
            messageHandler.Register(m_messageRegisterer);
            m_registeredHandlers.Add(messageHandlerType, messageHandler);
        }

        public void UnregisterHandler(IMessageHandler messageHandler)
        {
            Type messageHandlerType = messageHandler.GetType();
            if (!m_registeredHandlers.ContainsKey(messageHandlerType))
                return;
            m_registeredHandlers[messageHandlerType].Unregister(m_messageRegisterer);
            m_registeredHandlers.Remove(messageHandlerType);
        }

        public async Task<bool> Dispatch(LobbyServerMessage message)
        {
            if (!m_gameResponseHandlers.ContainsKey(message.GetType()))
                return false;
            object? result = ((Delegate)m_lobbyResponseHandlers[message.GetType()])?.DynamicInvoke(new object[] { message });
            if (result is not null)
            {
               return await (Task<bool>)result;
            }
            else
            {
               return false;
            }
        }

        public async Task<bool> Dispatch(GameServerMessage message)
        {
            if (!m_gameResponseHandlers.ContainsKey(message.GetType()))
               return false;
            object? result = ((Delegate)m_gameResponseHandlers[message.GetType()])?.DynamicInvoke(new object[] { message });
            if (result is not null)
            {
               return await (Task<bool>)result;
            }
            else
            {
               return false;
            }
        }

        private readonly Dictionary<Type, object> m_lobbyResponseHandlers = new Dictionary<Type, object>();
        private readonly Dictionary<Type, object> m_gameResponseHandlers = new Dictionary<Type, object>();
        private readonly IMessageRegisterer m_messageRegisterer;
        private readonly Dictionary<Type, IMessageHandler> m_registeredHandlers = new Dictionary<Type, IMessageHandler>();
    }
}
