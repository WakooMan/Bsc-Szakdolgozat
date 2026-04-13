namespace WebServer.Contract.Messages.Lobby.ServerMessages
{
    public class LeaveLobbyResponseMessage: LobbyServerMessage
    {
        public LeaveLobbyResponseMessage() : base() { }
        public LeaveLobbyResponseMessage(bool success, string message) : base(success, message) { }
    }
}
