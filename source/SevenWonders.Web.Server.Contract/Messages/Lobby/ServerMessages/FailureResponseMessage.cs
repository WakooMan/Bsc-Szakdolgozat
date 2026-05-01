namespace SevenWonders.Web.Server.Contract.Messages.Lobby.ServerMessages
{
    public class FailureResponseMessage: LobbyServerMessage
    {
        public FailureResponseMessage() : base() { }
        public FailureResponseMessage(bool success, string message) : base(success, message) { }
    }
}
