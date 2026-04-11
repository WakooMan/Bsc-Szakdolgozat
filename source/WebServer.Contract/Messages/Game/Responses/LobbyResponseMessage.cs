namespace WebServer.Contract.Messages.Game.Responses
{
    public class LobbyResponseMessage
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public LobbyResponseMessage() { Message = string.Empty; }
        public LobbyResponseMessage(bool success, string message)
        {
            Success = success;
            Message = message;
        }
    }
}
