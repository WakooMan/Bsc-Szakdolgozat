namespace WebServer.Contract.Messages.Lobby
{
    public class GameResponseMessage
    {
        public bool Success { get; set; }
        public GameResponseMessage() { }
        public GameResponseMessage(bool success)
        {
            Success = success;
        }
    }
}
