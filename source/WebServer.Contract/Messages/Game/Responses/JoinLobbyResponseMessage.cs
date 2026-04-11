using WebServer.Contract.DataTransferObjects;

namespace WebServer.Contract.Messages.Game.Responses
{
    public class JoinLobbyResponseMessage: LobbyResponseMessage
    {
        public LobbyDto LobbyDto { get; set; }
        public JoinLobbyResponseMessage()
        {
            LobbyDto = new LobbyDto();
        }

        public JoinLobbyResponseMessage(LobbyDto lobbyDto)
        {
            LobbyDto = lobbyDto;
        }
    }
}
