using System.Collections.Concurrent;
using WebServer.Contract.DataTransferObjects;
using WebServer.Model.Client;

namespace WebServer.Model.Lobby
{
    public interface ILobby
    {
        string Name { get; set; }
        string Code { get; }
        string HostConnectionId { get; set; }
        ConcurrentDictionary<string, IPlayerClient> Members { get; }
        ConcurrentQueue<ChatMessage> ChatMessages { get; }

        bool AddMember(IPlayerClient player);
        bool RemoveMember(IPlayerClient player);
        void AddChatMessage(ChatMessage chatMessage);

        LobbyDto ToDto();
    }
}
