namespace WebServer.Contract.Messages.Lobby.ServerMessages
{
    public class ExitGameResponseMessage: LobbyServerMessage
    {
        public ExitGameResponseMessage() : base() { }
        public ExitGameResponseMessage(bool success, string message) : base(success, message) { }
    }
}
