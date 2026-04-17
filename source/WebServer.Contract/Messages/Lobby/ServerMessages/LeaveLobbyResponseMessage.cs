using WebServer.Contract.DataTransferObjects;

namespace WebServer.Contract.Messages.Lobby.ServerMessages
{
    public class LeaveLobbyResponseMessage: LobbyServerMessage
    {
        public LobbyDto[] Lobbies { get; set; }
        public LeaveLobbyResponseMessage() : base() { Lobbies = []; }
        public LeaveLobbyResponseMessage(LobbyDto[] lobbies) : base(true, "OK") { Lobbies = lobbies; }
    }
}
