namespace SevenWonders.Web.Server.Model.MessageHandling.Factories
{
    public interface IMessageRegistererFactory
    {
        IMessageRegisterer Create(Dictionary<Type, List<object>> m_lobbyRequestHandlers, Dictionary<Type, List<object>> m_gameRequestHandlers);
    }
}
