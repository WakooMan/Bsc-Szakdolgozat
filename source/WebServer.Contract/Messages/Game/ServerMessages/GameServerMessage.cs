using System.Text.Json.Serialization;

namespace WebServer.Contract.Messages.Game.ServerMessages
{
    [JsonPolymorphic(TypeDiscriminatorPropertyName = "type")]
    [JsonDerivedType(typeof(FailureServerMessage), typeDiscriminator: "failureserver")]
    [JsonDerivedType(typeof(PlayerActionResponseMessage), typeDiscriminator: "playeractionresponse")]
    [JsonDerivedType(typeof(ServerPlayerActionMessage), typeDiscriminator: "serverplayeraction")]
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
