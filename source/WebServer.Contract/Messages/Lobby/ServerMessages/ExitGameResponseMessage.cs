using WebServer.Contract.DataTransferObjects;

namespace WebServer.Contract.Messages.Lobby.ServerMessages
{
    public class ExitGameResponseMessage: LobbyServerMessage
    {
        public LobbyDto[] Lobbies { get; set; }
        public ExitGameResponseMessage() : base() { Lobbies = []; }
        public ExitGameResponseMessage(LobbyDto[] lobbies) : base(true, "OK") { Lobbies = lobbies; }
    }
}
