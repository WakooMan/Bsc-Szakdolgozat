namespace WebServer.Model.Lobby
{
    public interface ILobbyCodeGenerator
    {
        string GenerateUniqueCode();

        bool RemoveUniqueCode(string code);
    }
}
