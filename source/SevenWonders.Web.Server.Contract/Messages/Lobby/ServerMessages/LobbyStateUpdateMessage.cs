using SevenWonders.Web.Server.Contract.DataTransferObjects;

namespace SevenWonders.Web.Server.Contract.Messages.Lobby.ServerMessages
{
    public class LobbyStateUpdateMessage : LobbyServerMessage
    {
        public LobbyDto LobbyDto { get; set; }

        public LobbyStateUpdateMessage() : base()
        {
            LobbyDto = new LobbyDto();
        }

        public LobbyStateUpdateMessage(LobbyDto lobbyDto) : base(true, "Success")
        {
            LobbyDto = lobbyDto;
        }
    }
}
