using WebServer.Contract.DataTransferObjects;

namespace WebServer.Contract.Messages.Lobby.ServerMessages
{
    public class LobbyUpdateMessage: LobbyServerMessage
    {
        public LobbyDto[] Lobbies { get; set; }

        public LobbyUpdateMessage(): base() { Lobbies = []; }

        public LobbyUpdateMessage(LobbyDto[] lobbies): base(true, "Success") { Lobbies = lobbies; }
    }
}
