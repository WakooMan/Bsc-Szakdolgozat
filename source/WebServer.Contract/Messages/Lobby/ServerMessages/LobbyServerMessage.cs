using System.Text.Json.Serialization;

namespace WebServer.Contract.Messages.Lobby.ServerMessages
{
    [JsonDerivedType(typeof(CreateLobbyResponseMessage), typeDiscriminator: "createlobbyresponse")]
    [JsonDerivedType(typeof(JoinLobbyResponseMessage), typeDiscriminator: "joinlobbyresponse")]
    [JsonDerivedType(typeof(StartGameResponseMessage), typeDiscriminator: "startgameresponse")]
    [JsonDerivedType(typeof(StartMatchmakingResponseMessage), typeDiscriminator: "startmatchmakingresponse")]
    [JsonDerivedType(typeof(StopMatchmakingResponseMessage), typeDiscriminator: "stopmatchmakingresponse")]
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
