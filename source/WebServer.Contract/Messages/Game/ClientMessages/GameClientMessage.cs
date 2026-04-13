using System.Text.Json.Serialization;

namespace WebServer.Contract.Messages.Game.ClientMessages
{
    [JsonPolymorphic(TypeDiscriminatorPropertyName = "type")]
    [JsonDerivedType(typeof(PlayerActionRequestMessage), typeDiscriminator: "playeractionrequest")]
    public abstract class GameClientMessage
    {
    }
}
