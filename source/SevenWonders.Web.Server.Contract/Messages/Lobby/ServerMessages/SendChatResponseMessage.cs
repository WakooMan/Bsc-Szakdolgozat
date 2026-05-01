namespace SevenWonders.Web.Server.Contract.Messages.Lobby.ServerMessages
{
    public class SendChatResponseMessage: LobbyServerMessage
    {
        public SendChatResponseMessage() : base() { }
        public SendChatResponseMessage(bool success, string message) : base(success, message) { }
    }
}
