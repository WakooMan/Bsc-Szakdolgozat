namespace WebServer.Contract.Messages.Lobby.ClientMessages
{
    public class CreateLobbyRequestMessage: LobbyClientMessage
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
