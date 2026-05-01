namespace SevenWonders.Web.Server.Contract.DataTransferObjects
{
    public class LobbySummaryDto
    {
        public LobbyDto[] Lobbies { get; set; }

        public LobbySummaryDto()
        {
            Lobbies = [];
        }

        public LobbySummaryDto(LobbyDto[] lobbies)
        {
            Lobbies = lobbies;
        }
    }
}
