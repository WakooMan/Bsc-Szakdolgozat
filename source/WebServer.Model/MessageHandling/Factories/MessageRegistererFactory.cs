namespace WebServer.Model.MessageHandling.Factories
{
    public class MessageRegistererFactory : IMessageRegistererFactory
    {
        public IMessageRegisterer Create(Dictionary<Type, object> m_lobbyRequestHandlers, Dictionary<Type, object> m_gameRequestHandlers)
        {
            return new MessageRegisterer(m_lobbyRequestHandlers, m_gameRequestHandlers);
        }
    }
}
