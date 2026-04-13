namespace SevenWonders.WebClient.Model.Factories
{
    public interface IMessageRegistererFactory
    {
        IMessageRegisterer Create(Dictionary<Type, object> m_lobbyRequestHandlers, Dictionary<Type, object> m_gameRequestHandlers);
    }
}
