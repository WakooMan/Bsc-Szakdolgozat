using System.Text.Json.Serialization;

namespace SevenWonders.Web.Server.Contract.Messages.Lobby.ClientMessages
{
    [JsonPolymorphic(TypeDiscriminatorPropertyName = "type")]
    [JsonDerivedType(typeof(CreateLobbyRequestMessage), typeDiscriminator: "createlobbyrequest")]
    [JsonDerivedType(typeof(JoinLobbyRequestMessage), typeDiscriminator: "joinlobbyrequest")]
    [JsonDerivedType(typeof(LeaveLobbyRequestMessage), typeDiscriminator: "leavelobbyrequest")]
    [JsonDerivedType(typeof(StartGameRequestMessage), typeDiscriminator: "startgamerequest")]
    [JsonDerivedType(typeof(StartMatchmakingRequestMessage), typeDiscriminator: "startmatchmakingrequest")]
    [JsonDerivedType(typeof(StopMatchmakingRequestMessage), typeDiscriminator: "stopmatchmakingrequest")]
    [JsonDerivedType(typeof(SendChatRequestMessage), typeDiscriminator: "sendchatrequest")]
    [JsonDerivedType(typeof(ExitGameRequestMessage), typeDiscriminator: "exitgamerequest")]
    [JsonDerivedType(typeof(GetLobbiesRequestMessage), typeDiscriminator: "getlobbiesrequest")]
    [JsonDerivedType(typeof(GetLeaderboardRequestMessage), typeDiscriminator: "getleaderboardrequest")]
    public abstract class LobbyClientMessage
    {
    }
}
