namespace WebServer.Contract.Messages.Lobby.ServerMessages
{
    public class StopMatchmakingResponseMessage : LobbyServerMessage
    {
        public StopMatchmakingResponseMessage() : base() { }
        public StopMatchmakingResponseMessage(bool success, string message) : base(success, message) { }
    }
}
