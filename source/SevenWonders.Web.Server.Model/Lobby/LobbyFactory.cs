using SevenWonders.Web.Server.Model.Client;

namespace SevenWonders.Web.Server.Model.Lobby
{
    public class LobbyFactory : ILobbyFactory
    {
        public LobbyFactory(IClientManager clientManager)
        {
            m_clientManager = clientManager;
        }
        public ILobby Create(string connectionId, string code, string name)
        {
            IPlayerClient playerClient = m_clientManager.GetClient(connectionId);
            return new Lobby(playerClient, name, code);
        }

        private readonly IClientManager m_clientManager;
    }
}
