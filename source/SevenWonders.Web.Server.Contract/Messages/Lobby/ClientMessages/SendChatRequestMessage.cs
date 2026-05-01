namespace SevenWonders.Web.Server.Contract.Messages.Lobby.ClientMessages
{
    public class SendChatRequestMessage : LobbyClientMessage
    {
        public string Message { get; set; }

        public SendChatRequestMessage()
        {
            Message = string.Empty;
        }

        public SendChatRequestMessage(string message)
        {
            Message = message;
        }
    }
}
