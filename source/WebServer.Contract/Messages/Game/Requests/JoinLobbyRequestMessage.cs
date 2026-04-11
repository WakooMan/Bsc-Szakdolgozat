namespace WebServer.Contract.Messages.Game.Requests
{
    public class JoinLobbyRequestMessage: LobbyRequestMessage
    {
        public string Code { get; set; }

        public JoinLobbyRequestMessage() { Code = string.Empty; }
        public JoinLobbyRequestMessage(string code) { Code = code; }
    }
}
