namespace WebServer.Contract.DataTransferObjects
{
    public class ChatMessage
    {
        public string UserName { get; set; }
        public string Message { get; set; }
        public DateTime TimeSpan { get; set; }

        public ChatMessage()
        {
            UserName = string.Empty;
            Message = string.Empty;
        }

        public ChatMessage(string userName, string message, DateTime timeSpan)
        {
            UserName = userName;
            Message = message;
            TimeSpan = timeSpan;
        }
    }
}
