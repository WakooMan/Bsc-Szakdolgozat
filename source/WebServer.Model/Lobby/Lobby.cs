using System.Collections.Concurrent;
using WebServer.Contract.DataTransferObjects;
using WebServer.Model.Client;

namespace WebServer.Model.Lobby
{
    public class Lobby: ILobby
    {
        public string Name { get; set; }
        public string Code { get; }
        public string HostConnectionId { get; set; }
        public ConcurrentDictionary<string, IPlayerClient> Members { get; }
        public ConcurrentQueue<ChatMessage> ChatMessages { get; }

        public Lobby(IPlayerClient host, string name, string code)
        {
            Name = name;
            Code = code;
            HostConnectionId = host.ConnectionId;
            Members = new ConcurrentDictionary<string, IPlayerClient>();
            Members.TryAdd(host.ConnectionId, host);
            ChatMessages = new ConcurrentQueue<ChatMessage>();
        }

        public bool AddMember(IPlayerClient player)
        {
            return Members.TryAdd(player.ConnectionId, player);
        }

        public bool RemoveMember(IPlayerClient player)
        {
            return Members.TryRemove(player.ConnectionId, out IPlayerClient? _);
        }

        public void AddChatMessage(ChatMessage chatMessage)
        {
            ChatMessages.Enqueue(chatMessage);
        }

        public LobbyDto ToDto()
        {
            return new LobbyDto(Name, Code, Members.Values.Select(playerClient => playerClient.ToDto(HostConnectionId == playerClient.ConnectionId)).ToArray(), ChatMessages.ToArray());
        }
    }
}
