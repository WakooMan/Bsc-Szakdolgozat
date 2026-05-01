namespace SevenWonders.Web.Client.Model.Factories
{
    public class MessageRegistererFactory : IMessageRegistererFactory
    {
        public IMessageRegisterer Create(Dictionary<Type, object> m_lobbyResponseHandlers, Dictionary<Type, object> m_gameResponseHandlers)
        {
            return new MessageRegisterer(m_lobbyResponseHandlers, m_gameResponseHandlers);
        }
    }
}
