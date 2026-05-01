using SevenWonders.Web.Server.Contract.DataTransferObjects;

namespace SevenWonders.Web.Server.Contract.Messages.Lobby.ServerMessages
{
    public class GetLeaderboardResponseMessage : LobbyServerMessage
    {
        public LeaderboardEntryDto[] Entries { get; set; }

        public GetLeaderboardResponseMessage() : base()
        {
            Entries = [];
        }

        public GetLeaderboardResponseMessage(bool success, string message, LeaderboardEntryDto[] entries) : base(success, message)
        {
            Entries = entries;
        }
    }
}
