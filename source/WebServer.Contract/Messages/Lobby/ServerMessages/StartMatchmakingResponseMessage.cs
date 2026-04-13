namespace WebServer.Contract.Messages.Lobby.ServerMessages
{
    public class StartMatchmakingResponseMessage : LobbyServerMessage
    {
        public StartMatchmakingResponseMessage() : base() { }
        public StartMatchmakingResponseMessage(bool success, string message) : base(success, message) { }
    }
}
