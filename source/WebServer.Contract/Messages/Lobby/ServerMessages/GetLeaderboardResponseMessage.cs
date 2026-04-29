using WebServer.Contract.DataTransferObjects;

namespace WebServer.Contract.Messages.Lobby.ServerMessages
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
