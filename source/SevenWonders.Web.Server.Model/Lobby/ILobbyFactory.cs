namespace SevenWonders.Web.Server.Model.Lobby
{
    public interface ILobbyFactory
    {
        ILobby Create(string connectionId, string code, string name);
    }
}
