namespace SevenWonders.Web.Server.Contract.Messages.Lobby.ServerMessages
{
    public class StartMatchmakingResponseMessage : LobbyServerMessage
    {
        public StartMatchmakingResponseMessage() : base() { }
        public StartMatchmakingResponseMessage(bool success, string message) : base(success, message) { }
    }
}
