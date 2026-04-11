using Microsoft.AspNetCore.SignalR;
using WebServer.Contract.Messages.Game.Requests;
using WebServer.Contract.Messages.Game.Responses;
using WebServer.Contract.Messages.Lobby;
using WebServer.Model.MessageHandling.Factories;

namespace WebServer.Model.MessageHandling
{
    public class ServerMessageDispatcher: IServerMessageDispatcher
    {
        public ServerMessageDispatcher(IMessageRegistererFactory messageRegistererFactory)
        {
            m_gameRequestHandlers = new Dictionary<Type, object>();
            m_lobbyRequestHandlers = new Dictionary<Type, object>();
            m_registeredHandlers = new Dictionary<Type, IMessageHandler>();
            m_messageRegisterer = messageRegistererFactory.Create(m_lobbyRequestHandlers, m_gameRequestHandlers);
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

        public async Task<LobbyResponseMessage> Dispatch(Hub hub, string connectionId, LobbyRequestMessage message)
        {
            if (!m_gameRequestHandlers.ContainsKey(message.GetType()))
                return await Task.FromResult(new LobbyResponseMessage(false, "Request message cannot be handled on the server side!"));
            object? result = ((Delegate)m_lobbyRequestHandlers[message.GetType()])?.DynamicInvoke(new object[] { hub, connectionId, message });
            if (result is not null)
            {
                return await (Task<LobbyResponseMessage>)result;
            }
            else
            {
                return await Task.FromResult(new LobbyResponseMessage(false, "Result is not returned from the handler!"));
            }
        }

        public async Task<GameResponseMessage> Dispatch(Hub hub, string connectionId, GameRequestMessage message)
        {
            if (!m_gameRequestHandlers.ContainsKey(message.GetType()))
                return await Task.FromResult(new GameResponseMessage(false));
            object? result = ((Delegate)m_gameRequestHandlers[message.GetType()])?.DynamicInvoke(new object[] { hub, connectionId, message });
            if (result is not null)
            {
                return await (Task<GameResponseMessage>)result;
            }
            else
            {
                return await Task.FromResult(new GameResponseMessage(false));
            }
        }

        private readonly Dictionary<Type, object> m_lobbyRequestHandlers = new Dictionary<Type, object>();
        private readonly Dictionary<Type, object> m_gameRequestHandlers = new Dictionary<Type, object>();
        private readonly IMessageRegisterer m_messageRegisterer;
        private readonly Dictionary<Type, IMessageHandler> m_registeredHandlers = new Dictionary<Type, IMessageHandler>();
    }
}
