using GameLogic;
using GameLogic.Elements;
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
        public InLobby(IPlayerStateFactory playerStateFactory, 
                       ILobbyManager lobbyManager, 
                       IPlayerClient player, 
                       IServerService serverService, 
                       ILobbyCodeGenerator lobbyCodeGenerator, 
                       IGameManager gameManager, 
                       IRandomGeneratorFactory randomGeneratorFactory,
                       string lobbyCode) : base(player, serverService, playerStateFactory, lobbyCodeGenerator)
        {
            m_lobbyManager = lobbyManager;
            m_gameManager = gameManager;
            m_lobbyCode = lobbyCode;
            m_randomGeneratorFactory = randomGeneratorFactory;
        }

        public override Task CreateLobby(string name)
        {
            throw new NotSupportedException("Cannot execute action in this state!");
        }

        public override Task ExitGame()
        {
            throw new NotSupportedException("Cannot execute action in this state!");
        }


        public override async Task WriteChatMessage(string message)
        {
            ILobby? lobby = m_lobbyManager.GetLobby(m_lobbyCode);
            if (lobby is null)
            {
                throw new InvalidOperationException("Cannot write chat in lobby, because it is not found!");
            }

            lobby.AddChatMessage(new ChatMessage(m_player.ApplicationUser.UserName ?? "Unknown", message, DateTime.UtcNow));
            await m_serverService.SendLobbyServerMessageToGroup($"{lobby.Code}", new LobbyStateUpdateMessage(lobby.ToDto()));
        }

        public override Task ExitMatchmaking()
        {
            throw new NotSupportedException("Cannot execute action in this state!");
        }

        public override Task JoinLobby(string code)
        {
            throw new NotSupportedException("Cannot execute action in this state!");
        }

        public override async Task LeaveLobby()
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
            await m_serverService.SendLobbyServerMessageToClient(m_player.ConnectionId, new LeaveLobbyResponseMessage(m_lobbyManager.GetLobbies().Select(lobby => lobby.ToDto()).ToArray()));
        }

        public override async Task StartGame()
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

            if(lobby.HostConnectionId != m_player.ConnectionId)
            {
                throw new InvalidOperationException("Only the host can start the game!");
            }

            m_lobbyManager.RemoveLobby(m_lobbyCode);
            foreach (var member in lobby.Members)
            {
                member.Value.ChangeState(m_playerStateFactory.CreateInGameState(member.Value, m_lobbyCode));
            }

            await m_serverService.SendLobbyServerMessageToGroup(nameof(InMainMenu), new LobbyUpdateMessage(m_lobbyManager.GetLobbies().Select(lobby => lobby.ToDto()).ToArray()));

            IPlayerClient otherPlayer = lobby.Members.First(m => m.Key != m_player.ConnectionId).Value;
            if (m_gameManager.AddGame(m_lobbyCode, out IGame? game) && 
                game is not null && 
                m_player.CurrentState is InGame playerState &&
                otherPlayer.CurrentState is InGame otherPlayerState)
            {
                IRandomGenerator randomGenerator = m_randomGeneratorFactory.Create(RandomGeneratorType.Undeterministic, 0);
                int seed = randomGenerator.Next();
                game.Initialize(
                    m_randomGeneratorFactory.Create(RandomGeneratorType.Deterministic, seed),
                    (m_player.ApplicationUser.UserName ?? string.Empty, playerState),
                    (otherPlayer.ApplicationUser.UserName ?? string.Empty, otherPlayerState));
                m_gameManager.StartGame(m_lobbyCode);
                await m_serverService.SendLobbyServerMessageToClient(otherPlayer.ConnectionId, new StartGameResponseMessage(otherPlayer.ApplicationUser.UserName ?? string.Empty, 
                                                                                                                            m_player.ApplicationUser.UserName ?? string.Empty, 
                                                                                                                            PlayerType.LocalPlayerWithRemoteOpponent, 
                                                                                                                            PlayerType.RemotePlayer, 
                                                                                                                            2, 
                                                                                                                            seed));
                await m_serverService.SendLobbyServerMessageToClient(m_player.ConnectionId, new StartGameResponseMessage(m_player.ApplicationUser.UserName ?? string.Empty, 
                                                                                                                         otherPlayer.ApplicationUser.UserName ?? string.Empty, 
                                                                                                                         PlayerType.LocalPlayerWithRemoteOpponent, 
                                                                                                                         PlayerType.RemotePlayer, 
                                                                                                                         1, 
                                                                                                                         seed));
            }
        }

        public override Task StartMatchmaking()
        {
            throw new NotSupportedException("Cannot execute action in this state!");
        }

        private readonly string m_lobbyCode;
        private readonly ILobbyManager m_lobbyManager;
        private readonly IGameManager m_gameManager;
        private readonly IRandomGeneratorFactory m_randomGeneratorFactory;
    }
}
