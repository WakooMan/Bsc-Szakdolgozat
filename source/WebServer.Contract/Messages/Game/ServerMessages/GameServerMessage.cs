using System.Text.Json.Serialization;

namespace WebServer.Contract.Messages.Game.ServerMessages
{
    [JsonDerivedType(typeof(FailureServerMessage), typeDiscriminator: "failureserver")]
    public abstract class GameServerMessage
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        protected GameServerMessage() { Message = string.Empty; }
        protected GameServerMessage(bool success, string message)
        {
            Success = success;
            Message = message;
        }
    }
}
