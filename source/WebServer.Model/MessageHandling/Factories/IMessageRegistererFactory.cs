namespace WebServer.Model.MessageHandling.Factories
{
    public interface IMessageRegistererFactory
    {
        IMessageRegisterer Create(Dictionary<Type, List<object>> m_lobbyRequestHandlers, Dictionary<Type, List<object>> m_gameRequestHandlers);
    }
}
