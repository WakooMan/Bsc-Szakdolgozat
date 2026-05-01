using System.Collections.Concurrent;
using SevenWonders.Web.Server.Contract.DataTransferObjects;
using SevenWonders.Web.Server.Model.Client;

namespace SevenWonders.Web.Server.Model.Lobby
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
