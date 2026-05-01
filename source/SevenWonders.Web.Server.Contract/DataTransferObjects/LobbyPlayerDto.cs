namespace SevenWonders.Web.Server.Contract.DataTransferObjects
{
    public class LobbyPlayerDto
    {

        public string UserName { get; set; }
        public bool IsHost { get; set; }

        public LobbyPlayerDto()
        {
            UserName = string.Empty;
        }

        public LobbyPlayerDto(string userName, bool isHost)
        {
            UserName = userName;
            IsHost = isHost;
        }
    }
}
