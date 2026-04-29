using WebServer.Model.Client;

namespace WebServer.Model.Matchmaking
{
    public class Match
    {
        public IPlayerClient Client { get; set; }
        public string GameCode { get; set; }

        public Match(IPlayerClient client, string gameCode)
        {
            Client = client;
            GameCode = gameCode;
        }
    }
}
