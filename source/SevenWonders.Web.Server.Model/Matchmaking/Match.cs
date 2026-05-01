using SevenWonders.Web.Server.Model.Client;

namespace SevenWonders.Web.Server.Model.Matchmaking
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
