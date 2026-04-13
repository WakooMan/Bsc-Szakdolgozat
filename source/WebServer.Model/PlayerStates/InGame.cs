using WebServer.Contract.Messages.Lobby.ServerMessages;
using WebServer.Model.Client;
using WebServer.Model.Lobby;
using WebServer.Model.PlayerStates.Factories;
using WebServer.Model.ServerHub;

namespace WebServer.Model.PlayerStates
{
    public class InGame : PlayerState
    {
        public InGame(IPlayerStateFactory playerStateFactory, IPlayerClient player, IServerService serverService, ILobbyCodeGenerator lobbyCodeGenerator, string gameCode) : base(player, serverService, playerStateFactory, lobbyCodeGenerator)
        {
            m_gameCode = gameCode;
        }

        public override Task<LobbyServerMessage> CreateLobby(string name)
        {
            throw new NotSupportedException("Cannot execute action in this state!");
        }

        public override async Task<LobbyServerMessage> ExitGame()
        {
            m_player.ChangeState(m_playerStateFactory.CreateInMainMenuState(m_player));
            await m_serverService.LeaveGroup(m_player.ConnectionId, m_gameCode);
            await m_serverService.JoinGroup(m_player.ConnectionId, nameof(InMainMenu));
            m_lobbyCodeGenerator.RemoveUniqueCode(m_gameCode);
            return new ExitGameResponseMessage(true, "OK");
        }

        public override Task<LobbyServerMessage> ExitMatchmaking()
        {
            throw new NotSupportedException("Cannot execute action in this state!");
        }

        public override Task<LobbyServerMessage> JoinLobby(string code)
        {
            throw new NotSupportedException("Cannot execute action in this state!");
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
            throw new NotSupportedException("Cannot execute action in this state!");
        }

        public override Task<LobbyServerMessage> WriteChatMessage(string message)
        {
            throw new NotSupportedException("Cannot execute action in this state!");
        }

        private readonly string m_gameCode;
    }
}
