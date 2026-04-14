using SevenWonders.Common;

namespace WebServer.Contract.Messages.Lobby.ServerMessages
{
    public class StartGameResponseMessage: LobbyServerMessage
    {
        public PlayerInitModel Player1 { get; set; }

        public PlayerInitModel Player2 { get; set; }
        public int StartingPlayerId { get; set; }
        public StartGameResponseMessage() :base() { Player1 = new(); Player2 = new(); StartingPlayerId = 0; }
        public StartGameResponseMessage(string message) : base(false, message) { Player1 = new(); Player2 = new(); StartingPlayerId = 0; }

        public StartGameResponseMessage(PlayerInitModel player1, PlayerInitModel player2, int startingPlayerId) : base(true, "Success")
        {
            Player1 = player1;
            Player2 = player2;
            StartingPlayerId = startingPlayerId;
        }
    }
}
