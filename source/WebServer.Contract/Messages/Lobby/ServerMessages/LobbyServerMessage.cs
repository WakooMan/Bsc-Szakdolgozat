using System.Text.Json.Serialization;

namespace WebServer.Contract.Messages.Lobby.ServerMessages
{
    [JsonPolymorphic(TypeDiscriminatorPropertyName = "type")]
    [JsonDerivedType(typeof(CreateLobbyResponseMessage), typeDiscriminator: "createlobbyresponse")]
    [JsonDerivedType(typeof(JoinLobbyResponseMessage), typeDiscriminator: "joinlobbyresponse")]
    [JsonDerivedType(typeof(StartGameResponseMessage), typeDiscriminator: "startgameresponse")]
    [JsonDerivedType(typeof(StartMatchmakingResponseMessage), typeDiscriminator: "startmatchmakingresponse")]
    [JsonDerivedType(typeof(StopMatchmakingResponseMessage), typeDiscriminator: "stopmatchmakingresponse")]
    [JsonDerivedType(typeof(LobbyUpdateMessage), typeDiscriminator: "lobbyupdatemessage")]
    [JsonDerivedType(typeof(LeaveLobbyResponseMessage), typeDiscriminator: "leavelobbyresponse")]
    [JsonDerivedType(typeof(LobbyStateUpdateMessage), typeDiscriminator: "lobbystateupdatemessage")]
    [JsonDerivedType(typeof(ExitGameResponseMessage), typeDiscriminator: "exitgameresponse")]
    [JsonDerivedType(typeof(SendChatResponseMessage), typeDiscriminator: "sendchatresponse")]
    [JsonDerivedType(typeof(FailureResponseMessage), typeDiscriminator: "failureresponse")]
    [JsonDerivedType(typeof(GetLeaderboardResponseMessage), typeDiscriminator: "getleaderboardresponse")]
    public abstract class LobbyServerMessage
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        protected LobbyServerMessage() { Message = string.Empty; }
        protected LobbyServerMessage(bool success, string message)
        {
            Success = success;
            Message = message;
        }
    }
}
