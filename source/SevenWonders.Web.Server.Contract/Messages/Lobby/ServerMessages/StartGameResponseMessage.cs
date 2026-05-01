using SevenWonders.Common;

namespace SevenWonders.Web.Server.Contract.Messages.Lobby.ServerMessages
{
    public class StartGameResponseMessage : LobbyServerMessage
    {
        public string Player1Name { get; set; }
        public string Player2Name { get; set; }
        public PlayerType Player1Type { get; set; }
        public PlayerType Player2Type { get; set; }
        public int StartingPlayerId { get; set; }
        public int Seed { get; set; }

        public StartGameResponseMessage() : base() { Player1Name = string.Empty; Player2Name = string.Empty; Player1Type = PlayerType.Unknown; Player2Type = PlayerType.Unknown; StartingPlayerId = 0; Seed = 0; }
        public StartGameResponseMessage(string message) : base(false, message) { Player1Name = string.Empty; Player2Name = string.Empty; Player1Type = PlayerType.Unknown; Player2Type = PlayerType.Unknown; StartingPlayerId = 0; Seed = 0; }

        public StartGameResponseMessage(string player1Name, string player2Name, PlayerType player1Type, PlayerType player2Type, int startingPlayerId, int seed) : base(true, "Success")
        {
            Player1Name = player1Name;
            Player2Name = player2Name;
            Player1Type = player1Type;
            Player2Type = player2Type;
            StartingPlayerId = startingPlayerId;
            Seed = seed;
        }
    }
}
