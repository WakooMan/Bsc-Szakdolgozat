namespace SevenWonders.Web.Server.Contract.Messages.Lobby.ClientMessages
{
    public class JoinLobbyRequestMessage: LobbyClientMessage
    {
        public string Code { get; set; }

        public JoinLobbyRequestMessage() { Code = string.Empty; }
        public JoinLobbyRequestMessage(string code) { Code = code; }
    }
}
