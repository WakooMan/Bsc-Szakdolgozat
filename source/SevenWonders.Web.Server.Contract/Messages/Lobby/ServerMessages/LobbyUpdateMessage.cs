using SevenWonders.Web.Server.Contract.DataTransferObjects;

namespace SevenWonders.Web.Server.Contract.Messages.Lobby.ServerMessages
{
    public class LobbyUpdateMessage: LobbyServerMessage
    {
        public LobbyDto[] Lobbies { get; set; }

        public LobbyUpdateMessage(): base() { Lobbies = []; }

        public LobbyUpdateMessage(LobbyDto[] lobbies): base(true, "Success") { Lobbies = lobbies; }
    }
}
