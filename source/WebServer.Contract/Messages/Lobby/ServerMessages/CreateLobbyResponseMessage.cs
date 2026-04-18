using WebServer.Contract.DataTransferObjects;

namespace WebServer.Contract.Messages.Lobby.ServerMessages
{
    public class CreateLobbyResponseMessage: LobbyServerMessage
    {
        public LobbyDto LobbyDto { get; set; }
        public CreateLobbyResponseMessage(): base()
        {
            LobbyDto = new LobbyDto();
        }

        public CreateLobbyResponseMessage(bool success, string message, LobbyDto lobbyDto): base(success, message)
        {
            LobbyDto = lobbyDto;
        }
    }
}
