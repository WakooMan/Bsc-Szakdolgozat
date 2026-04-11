using System.Collections.Concurrent;

namespace WebServer.Model.Client
{
    public class ClientManager : IClientManager
    {
        public ClientManager()
        {
            m_clients = new ConcurrentDictionary<string, IPlayerClient> ();
        }

        public bool AddClient(IPlayerClient client)
        {
            return m_clients.TryAdd(client.ConnectionId, client);
        }

        public IPlayerClient GetClient(string connectionId)
        {
            return m_clients[connectionId];
        }

        public IPlayerClient[] GetClients()
        {
            return m_clients.Values.ToArray();
        }

        public bool RemoveClient(IPlayerClient client)
        {
            return m_clients.TryRemove(client.ConnectionId, out IPlayerClient? _);
        }

        private readonly ConcurrentDictionary<string, IPlayerClient> m_clients;
    }
}
