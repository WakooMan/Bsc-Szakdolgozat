using System.Text.Json.Serialization;

namespace SevenWonders.Web.Server.Contract.Messages.Game.ClientMessages
{
    [JsonPolymorphic(TypeDiscriminatorPropertyName = "type")]
    [JsonDerivedType(typeof(PlayerActionRequestMessage), typeDiscriminator: "playeractionrequest")]
    public abstract class GameClientMessage
    {
    }
}
