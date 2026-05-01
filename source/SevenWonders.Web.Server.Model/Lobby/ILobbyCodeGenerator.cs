namespace SevenWonders.Web.Server.Model.Lobby
{
    public interface ILobbyCodeGenerator
    {
        string GenerateUniqueCode();

        bool RemoveUniqueCode(string code);
    }
}
