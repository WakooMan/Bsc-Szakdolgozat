namespace SevenWonders.Web.Server.Model.MessageHandling.Factories
{
    public class MessageRegistererFactory : IMessageRegistererFactory
    {
        public IMessageRegisterer Create(Dictionary<Type, List<object>> m_lobbyRequestHandlers, Dictionary<Type, List<object>> m_gameRequestHandlers)
        {
            return new MessageRegisterer(m_lobbyRequestHandlers, m_gameRequestHandlers);
        }
    }
}
