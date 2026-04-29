namespace WebServer.Contract.DataTransferObjects
{
    public class LeaderboardEntryDto
    {
        public string UserName { get; set; }
        public int Wins { get; set; }

        public LeaderboardEntryDto()
        {
            UserName = string.Empty;
            Wins = 0;
        }

        public LeaderboardEntryDto(string userName, int wins)
        {
            UserName = userName;
            Wins = wins;
        }
    }
}
