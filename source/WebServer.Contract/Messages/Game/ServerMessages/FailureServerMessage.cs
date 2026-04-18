namespace WebServer.Contract.Messages.Game.ServerMessages
{
    public class FailureServerMessage: GameServerMessage
    {
        public FailureServerMessage() : base() { }
        public FailureServerMessage(bool success, string message) : base(success, message) { }
    }
}
