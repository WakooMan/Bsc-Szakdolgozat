using System.Collections.Concurrent;

namespace SevenWonders.Web.Server.Model.Lobby
{
    public class LobbyManager : ILobbyManager
    {
        public ConcurrentDictionary<string, ILobby> Lobbies { get; }

        public LobbyManager(ILobbyFactory lobbyFactory)
        {
            Lobbies = new ConcurrentDictionary<string, ILobby>();
            m_lobbyFactory = lobbyFactory;
        }
        public bool AddLobby(string connectionId, string code, string name, out ILobby lobby)
        {
            lobby = m_lobbyFactory.Create(connectionId, code, name);
            return Lobbies.TryAdd(code, lobby);
        }

        public ILobby? GetLobby(string code)
        {
            if (Lobbies.ContainsKey(code))
            {
                return Lobbies[code];
            }
            return null;
        }

        public bool RemoveLobby(string code)
        {
            return Lobbies.TryRemove(code, out ILobby? _);
        }

        public ILobby[] GetLobbies()
        {
            return Lobbies.Values.ToArray();
        }

        public string[] GetLobbyCodes()
        {
            return Lobbies.Keys.ToArray();
        }

        private readonly ILobbyFactory m_lobbyFactory;
    }
}
