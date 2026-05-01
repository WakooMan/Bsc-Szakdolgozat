namespace SevenWonders.Web.Server.Model.Lobby
{
    public interface ILobbyManager
    {
        public ILobby? GetLobby(string code);
        public ILobby[] GetLobbies();

        public string[] GetLobbyCodes();
        public bool AddLobby(string connectionId, string code, string name, out ILobby lobby);
        public bool RemoveLobby(string code);
    }
}
