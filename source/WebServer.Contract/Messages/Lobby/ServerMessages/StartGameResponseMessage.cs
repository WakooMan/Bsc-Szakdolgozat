namespace WebServer.Contract.Messages.Lobby.ServerMessages
{
    public class StartGameResponseMessage: LobbyServerMessage
    {
        public StartGameResponseMessage() :base() { }
        public StartGameResponseMessage(bool success, string message) : base(success, message) { }
    }
}
