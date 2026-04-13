using Microsoft.AspNetCore.SignalR;
using WebServer.Contract.Messages.Lobby.ClientMessages;
using WebServer.Contract.Messages.Lobby.ServerMessages;
using WebServer.Contract.Messages.Lobby;
using WebServer.Model.MessageHandling.Factories;
using WebServer.Contract.Messages.Game.ServerMessages;
using WebServer.Contract.Messages.Game.ClientMessages;

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

        public async Task<LobbyServerMessage> Dispatch(Hub hub, string connectionId, LobbyClientMessage message)
        {
            if (!m_gameRequestHandlers.ContainsKey(message.GetType()))
                return await Task.FromResult(new FailureResponseMessage(false, "Request message cannot be handled on the server side!"));
            object? result = ((Delegate)m_lobbyRequestHandlers[message.GetType()])?.DynamicInvoke(new object[] { hub, connectionId, message });
            if (result is not null)
            {
                return await (Task<LobbyServerMessage>)result;
            }
            else
            {
                return await Task.FromResult(new FailureResponseMessage(false, "Result is not returned from the handler!"));
            }
        }

        public async Task<GameServerMessage> Dispatch(Hub hub, string connectionId, GameClientMessage message)
        {
            if (!m_gameRequestHandlers.ContainsKey(message.GetType()))
                return await Task.FromResult(new FailureServerMessage(false, "Request message cannot be handled on the server side!"));
            object? result = ((Delegate)m_gameRequestHandlers[message.GetType()])?.DynamicInvoke(new object[] { hub, connectionId, message });
            if (result is not null)
            {
                return await (Task<GameServerMessage>)result;
            }
            else
            {
                return await Task.FromResult(new FailureServerMessage(false, "Result is not returned from the handler!"));
            }
        }

        private readonly Dictionary<Type, object> m_lobbyRequestHandlers = new Dictionary<Type, object>();
        private readonly Dictionary<Type, object> m_gameRequestHandlers = new Dictionary<Type, object>();
        private readonly IMessageRegisterer m_messageRegisterer;
        private readonly Dictionary<Type, IMessageHandler> m_registeredHandlers = new Dictionary<Type, IMessageHandler>();
    }
}
