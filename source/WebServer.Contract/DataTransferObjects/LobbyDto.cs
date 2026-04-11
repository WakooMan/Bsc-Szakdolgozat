namespace WebServer.Contract.DataTransferObjects
{
    public class LobbyDto
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public LobbyPlayerDto[] Members { get; set; }
        public ChatMessage[] ChatMessages { get; set; }

        public LobbyDto()
        {
            Name = string.Empty;
            Code = string.Empty;
            Members = [];
            ChatMessages = [];
        }

        public LobbyDto(string name, string code, LobbyPlayerDto[] members, ChatMessage[] chatMessages)
        {
            Name = name;
            Code = code;
            Members = members;
            ChatMessages = chatMessages;
        }
    }
}
