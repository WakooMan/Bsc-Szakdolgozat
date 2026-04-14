using GameLogic;
using SevenWonders.Common;
using WebServer.Contract.DataTransferObjects;
using WebServer.Contract.Messages.Lobby.ServerMessages;
using WebServer.Model.Client;
using WebServer.Model.Lobby;
using WebServer.Model.PlayerStates.Factories;
using WebServer.Model.ServerHub;

namespace WebServer.Model.PlayerStates
{
    public class InLobby : PlayerState
    {
        public InLobby(IPlayerStateFactory playerStateFactory, ILobbyManager lobbyManager, IPlayerClient player, IServerService serverService, ILobbyCodeGenerator lobbyCodeGenerator, IGameManager gameManager, string lobbyCode) : base(player, serverService, playerStateFactory, lobbyCodeGenerator)
        {
            m_lobbyManager = lobbyManager;
            m_lobbyCode = lobbyCode;
        }

        public override Task<LobbyServerMessage> CreateLobby(string name)
        {
            throw new NotSupportedException("Cannot execute action in this state!");
        }

        public override Task<LobbyServerMessage> ExitGame()
        {
            throw new NotSupportedException("Cannot execute action in this state!");
        }


        public override async Task<LobbyServerMessage> WriteChatMessage(string message)
        {
            ILobby? lobby = m_lobbyManager.GetLobby(m_lobbyCode);
            if (lobby is null)
            {
                throw new InvalidOperationException("Cannot write chat in lobby, because it is not found!");
            }

            lobby.AddChatMessage(new ChatMessage(m_player.ApplicationUser.UserName ?? "Unknown", message, DateTime.UtcNow));
            await m_serverService.SendLobbyServerMessageToGroup($"{lobby.Code}", new LobbyStateUpdateMessage(lobby.ToDto()));
            return new SendChatResponseMessage(true, "OK");
        }

        public override Task<LobbyServerMessage> ExitMatchmaking()
        {
            throw new NotSupportedException("Cannot execute action in this state!");
        }

        public override Task<LobbyServerMessage> JoinLobby(string code)
        {
            throw new NotSupportedException("Cannot execute action in this state!");
        }

        public override async Task<LobbyServerMessage> LeaveLobby()
        {
            ILobby? lobby = m_lobbyManager.GetLobby(m_lobbyCode);
            if (lobby is null)
            {
                throw new InvalidOperationException("Cannot leave lobby, because it is not found!");
            }

            lobby.RemoveMember(m_player);
            if (lobby.Members.Count < 1)
            {
                m_lobbyManager.RemoveLobby(m_lobbyCode);
                m_lobbyCodeGenerator.RemoveUniqueCode(m_lobbyCode);
                await m_serverService.SendLobbyServerMessageToGroup(nameof(InMainMenu), new LobbyUpdateMessage(m_lobbyManager.GetLobbies().Select(lobby => lobby.ToDto()).ToArray()));
            }
            else if(lobby.HostConnectionId == m_player.ConnectionId)
            {
                lobby.HostConnectionId = lobby.Members.First().Key;
            }

            m_player.ChangeState(m_playerStateFactory.CreateInMainMenuState(m_player));
            await m_serverService.LeaveGroup(m_player.ConnectionId, m_lobbyCode);
            await m_serverService.JoinGroup(m_player.ConnectionId, nameof(InMainMenu));
            await m_serverService.SendLobbyServerMessageToGroup($"{lobby.Code}", new LobbyStateUpdateMessage(lobby.ToDto()));
            return new LeaveLobbyResponseMessage(true, "OK", m_lobbyManager.GetLobbies().Select(lobby => lobby.ToDto()).ToArray());
        }

        public override async Task<LobbyServerMessage> StartGame()
        {
            ILobby? lobby = m_lobbyManager.GetLobby(m_lobbyCode);

            if (lobby is null)
            {
                throw new InvalidOperationException("Cannot start game from lobby, because it is not found!");
            }

            if (lobby.Members.Count < 2)
            {
                throw new InvalidOperationException("Cannot start game, because there are not enough players in the lobby!");
            }

            m_lobbyManager.RemoveLobby(m_lobbyCode);
            foreach (var member in lobby.Members)
            {
                member.Value.ChangeState(m_playerStateFactory.CreateInGameState(member.Value, m_lobbyCode));
            }

            await m_serverService.SendLobbyServerMessageToGroup(nameof(InMainMenu), new LobbyUpdateMessage(m_lobbyManager.GetLobbies().Select(lobby => lobby.ToDto()).ToArray()));

            IPlayerClient otherPlayer = lobby.Members.First(m => m.Key != m_player.ConnectionId).Value;
            m_gameManager.AddGame(m_player, otherPlayer, m_lobbyCode, out IGame _);
            m_gameManager.StartGame(m_lobbyCode);
            await m_serverService.SendLobbyServerMessageToClient(otherPlayer.ConnectionId, new StartGameResponseMessage(new PlayerInitModel(otherPlayer.ApplicationUser.UserName, PlayerType.LocalPlayerWithRemoteOpponent), 
                                                                                                                        new PlayerInitModel(m_player.ApplicationUser.UserName, PlayerType.RemotePlayer)));
            return new StartGameResponseMessage(new PlayerInitModel(m_player.ApplicationUser.UserName, PlayerType.LocalPlayerWithRemoteOpponent), 
                                                new PlayerInitModel(otherPlayer.ApplicationUser.UserName, PlayerType.RemotePlayer));
        }

        public override Task<LobbyServerMessage> StartMatchmaking()
        {
            throw new NotSupportedException("Cannot execute action in this state!");
        }

        private readonly string m_lobbyCode;
        private readonly ILobbyManager m_lobbyManager;
        private readonly IGameManager m_gameManager;
    }
}
