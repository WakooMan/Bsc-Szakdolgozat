using WebServer.Contract.DataTransferObjects;

namespace WebServer.Contract.Messages.Lobby.ServerMessages
{
    public class LeaveLobbyResponseMessage: LobbyServerMessage
    {
        public LobbyDto[] Lobbies { get; set; }
        public LeaveLobbyResponseMessage() : base() { Lobbies = []; }
        public LeaveLobbyResponseMessage(bool success, string message, LobbyDto[] lobbies) : base(success, message) { Lobbies = lobbies; }
    }
}
