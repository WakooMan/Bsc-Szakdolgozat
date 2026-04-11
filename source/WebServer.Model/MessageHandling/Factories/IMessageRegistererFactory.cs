namespace WebServer.Model.MessageHandling.Factories
{
    public interface IMessageRegistererFactory
    {
        IMessageRegisterer Create(Dictionary<Type, object> m_lobbyRequestHandlers, Dictionary<Type, object> m_gameRequestHandlers);
    }
}
