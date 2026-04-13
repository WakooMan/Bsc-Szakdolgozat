using WebServer.Contract.Messages.Lobby.ServerMessages;
using WebServer.Model.Client;
using WebServer.Model.Lobby;
using WebServer.Model.PlayerStates.Factories;
using WebServer.Model.ServerHub;

namespace WebServer.Model.PlayerStates
{
    public class InMatchmaking : PlayerState
    {
        public InMatchmaking(IPlayerStateFactory playerStateFactory, IPlayerClient player, IServerService serverService, ILobbyCodeGenerator lobbyCodeGenerator) : base(player, serverService, playerStateFactory, lobbyCodeGenerator) { }

        public override Task<LobbyServerMessage> CreateLobby(string name)
        {
            throw new NotSupportedException("Cannot execute action in this state!");
        }

        public override Task<LobbyServerMessage> ExitGame()
        {
            throw new NotSupportedException("Cannot execute action in this state!");
        }

        public override Task<LobbyServerMessage> WriteChatMessage(string message)
        {
            throw new NotSupportedException("Cannot execute action in this state!");
        }

        public override Task<LobbyServerMessage> ExitMatchmaking()
        {
            m_player.ChangeState(m_playerStateFactory.CreateInMainMenuState(m_player));
            return Task.FromResult<LobbyServerMessage>(new StopMatchmakingResponseMessage(true, "OK"));
        }

        public override Task<LobbyServerMessage> JoinLobby(string code)
        {
            throw new NotSupportedException("Cannot execute action in this state!");
        }

        public override Task<LobbyServerMessage> LeaveLobby()
        {
            throw new NotSupportedException("Cannot execute action in this state!");
        }

        public override async Task<LobbyServerMessage> StartGame()
        {
            string code = m_lobbyCodeGenerator.GenerateUniqueCode();
            m_player.ChangeState(m_playerStateFactory.CreateInGameState(m_player, code));
            await m_serverService.LeaveGroup(m_player.ConnectionId, nameof(InMainMenu));
            await m_serverService.JoinGroup(m_player.ConnectionId, code);
            return new StartGameResponseMessage("OK");
        }

        public override Task<LobbyServerMessage> StartMatchmaking()
        {
            throw new NotSupportedException("Cannot execute action in this state!");
        }
    }
}
