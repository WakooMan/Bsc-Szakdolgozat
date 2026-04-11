namespace WebServer.Contract.Messages.Game.Requests
{
    public class CreateLobbyRequestMessage: LobbyRequestMessage
    {
        public string Name { get; set; }

        public CreateLobbyRequestMessage()
        {
            Name = string.Empty;
        }

        public CreateLobbyRequestMessage(string name)
        {
            Name = name;
        }
    }
}
