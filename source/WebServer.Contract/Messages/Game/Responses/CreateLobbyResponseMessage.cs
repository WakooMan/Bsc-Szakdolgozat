using WebServer.Contract.DataTransferObjects;

namespace WebServer.Contract.Messages.Game.Responses
{
    public class CreateLobbyResponseMessage: LobbyResponseMessage
    {
        public LobbyDto LobbyDto { get; set; }
        public CreateLobbyResponseMessage()
        {
            LobbyDto = new LobbyDto();
        }

        public CreateLobbyResponseMessage(LobbyDto lobbyDto)
        {
            LobbyDto = lobbyDto;
        }
    }
}
