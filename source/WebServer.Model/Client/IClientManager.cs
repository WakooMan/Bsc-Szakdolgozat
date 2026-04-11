namespace WebServer.Model.Client
{
    public interface IClientManager
    {
        public IPlayerClient GetClient(string connectionId);
        public bool AddClient(IPlayerClient client);
        public bool RemoveClient(IPlayerClient client);
        public IPlayerClient[] GetClients();
    }
}
