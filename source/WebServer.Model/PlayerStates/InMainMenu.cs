using Microsoft.AspNetCore.SignalR;
using WebServer.Contract.Messages.Lobby.ServerMessages;
using WebServer.Model.Client;
using WebServer.Model.Lobby;
using WebServer.Model.PlayerStates.Factories;
using WebServer.Model.ServerHub;

namespace WebServer.Model.PlayerStates
{
    public class InMainMenu : PlayerState
    {
        public InMainMenu(ILobbyManager lobbyManager, IPlayerStateFactory playerStateFactory, IPlayerClient player, IServerService serverService, ILobbyCodeGenerator lobbyCodeGenerator) : base(player, serverService, playerStateFactory, lobbyCodeGenerator)
        {
            m_lobbyManager = lobbyManager;
        }

        public override async Task<LobbyServerMessage> CreateLobby(string name)
        {
            string code = m_lobbyCodeGenerator.GenerateUniqueCode();
            ILobby? lobby = m_lobbyManager.GetLobby(code);
            if (lobby is not null)
            {
                throw new InvalidOperationException($"Cannot create lobby, because lobby with code: {code} already exists!");
            }

            if (!m_lobbyManager.AddLobby(m_player.ConnectionId, code, name, out ILobby result))
            {
                throw new InvalidOperationException("Could not add lobby to the lobby manager!");
            }

            m_player.ChangeState(m_playerStateFactory.CreateInLobbyState(m_player, code));
            await m_serverService.LeaveGroup(m_player.ConnectionId, nameof(InMainMenu));
            await m_serverService.JoinGroup(m_player.ConnectionId, code);
            await m_serverService.SendLobbyServerMessageToGroup(nameof(InMainMenu), new LobbyUpdateMessage(m_lobbyManager.GetLobbies().Select(lobby => lobby.ToDto()).ToArray()));
            return new CreateLobbyResponseMessage(true, "Success", result.ToDto());
        }

        public override Task<LobbyServerMessage> ExitGame()
        {
            throw new NotSupportedException("Cannot execute action in this state!");
        }

        public override Task<LobbyServerMessage> ExitMatchmaking()
        {
            throw new NotSupportedException("Cannot execute action in this state!");
        }

        public override Task<LobbyServerMessage> WriteChatMessage(string message)
        {
            throw new NotSupportedException("Cannot execute action in this state!");
        }

        public override async Task<LobbyServerMessage> JoinLobby(string code)
        {
            ILobby? lobby = m_lobbyManager.GetLobby(code);
            if (lobby is null)
            {
                throw new InvalidOperationException($"Lobby with code: {code} does not exist!");
            }

            if (lobby.Members.Count < 2)
            {
                lobby.AddMember(m_player);
                m_player.ChangeState(m_playerStateFactory.CreateInLobbyState(m_player, code));
                await m_serverService.SendLobbyServerMessageToGroup($"{lobby.Code}", new LobbyStateUpdateMessage(lobby.ToDto()));
                await m_serverService.LeaveGroup(m_player.ConnectionId, nameof(InMainMenu));
                await m_serverService.JoinGroup(m_player.ConnectionId, code);
                return new JoinLobbyResponseMessage(true, "Success", lobby.ToDto());
            }

            throw new InvalidOperationException("The lobby is filled!");
        }

        public override Task<LobbyServerMessage> LeaveLobby()
        {
            throw new NotSupportedException("Cannot execute action in this state!");
        }

        public override Task<LobbyServerMessage> StartGame()
        {
            throw new NotSupportedException("Cannot execute action in this state!");
        }

        public override Task<LobbyServerMessage> StartMatchmaking()
        {
            m_player.ChangeState(m_playerStateFactory.CreateInMatchmakingState(m_player));
            return Task.FromResult<LobbyServerMessage>(new StartMatchmakingResponseMessage(true, "OK"));
        }

        private readonly ILobbyManager m_lobbyManager;
    }
}
