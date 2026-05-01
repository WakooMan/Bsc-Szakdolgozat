using SevenWonders.Web.Server.Contract.DataTransferObjects;

namespace SevenWonders.Web.Server.Contract.Messages.Lobby.ServerMessages
{
    public class ExitGameResponseMessage: LobbyServerMessage
    {
        public LobbyDto[] Lobbies { get; set; }
        public ExitGameResponseMessage() : base() { Lobbies = []; }
        public ExitGameResponseMessage(LobbyDto[] lobbies) : base(true, "OK") { Lobbies = lobbies; }
    }
}
