using SevenWonders.Web.Server.Contract.DataTransferObjects;

namespace SevenWonders.Web.Server.Contract.Messages.Lobby.ServerMessages
{
    public class JoinLobbyResponseMessage: LobbyServerMessage
    {
        public LobbyDto LobbyDto { get; set; }
        public JoinLobbyResponseMessage(): base()
        {
            LobbyDto = new LobbyDto();
        }

        public JoinLobbyResponseMessage(bool success, string message, LobbyDto lobbyDto): base(success, message)
        {
            LobbyDto = lobbyDto;
        }
    }
}
