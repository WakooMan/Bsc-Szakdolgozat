using System.Text.Json.Serialization;

namespace WebServer.Contract.Messages.Game.Requests
{
    [JsonDerivedType(typeof(CreateLobbyRequestMessage), typeDiscriminator: "createlobbyrequest")]
    [JsonDerivedType(typeof(JoinLobbyRequestMessage), typeDiscriminator: "joinlobbyrequest")]
    [JsonDerivedType(typeof(LeaveLobbyRequestMessage), typeDiscriminator: "createlobbyrequest")]
    [JsonDerivedType(typeof(StartGameRequestMessage), typeDiscriminator: "startgamerequest")]
    [JsonDerivedType(typeof(StartMatchmakingRequestMessage), typeDiscriminator: "startmatchmakingrequest")]
    [JsonDerivedType(typeof(StopMatchmakingRequestMessage), typeDiscriminator: "stopmatchmakingrequest")]
    public abstract class LobbyRequestMessage
    {
    }
}
