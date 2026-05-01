using SevenWonders.Game.Logic;
using SevenWonders.Game.Logic.Elements;
using SevenWonders.Game.Logic.Elements.GameCards;
using SevenWonders.Game.Logic.Events.GameEvents;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SevenWonders.Common;
using SevenWonders.Web.Server.Contract.Messages.Lobby.ServerMessages;
using SevenWonders.Web.Server.Model.Client;
using SevenWonders.Web.Server.Model.Lobby;
using SevenWonders.Web.Server.Model.Matchmaking;
using SevenWonders.Web.Server.Model.PlayerStates.Factories;
using SevenWonders.Web.Server.Model.ServerHub;

namespace SevenWonders.Web.Server.Model.PlayerStates
{
    public class InMatchmaking : PlayerState
    {
        public InMatchmaking(IPlayerStateFactory playerStateFactory, 
                             IPlayerClient player, 
                             IServerService serverService, 
                             ILobbyCodeGenerator lobbyCodeGenerator, 
                             IMatchmakingService matchmakingService,
                             IRandomGeneratorFactory randomGeneratorFactory,
                             IServiceScopeFactory serviceScopeFactory,
                             IGameManager gameManager) : base(player, serverService, playerStateFactory, lobbyCodeGenerator)
        {
            m_matchmakingService = matchmakingService;
            m_randomGeneratorFactory = randomGeneratorFactory;
            m_serviceScopeFactory = serviceScopeFactory;
            m_gameManager = gameManager;
        }

        public override Task CreateLobby(string name)
        {
            throw new NotSupportedException("Cannot execute action in this state!");
        }

        public override Task ExitGame()
        {
            throw new NotSupportedException("Cannot execute action in this state!");
        }

        public override Task WriteChatMessage(string message)
        {
            throw new NotSupportedException("Cannot execute action in this state!");
        }

        public override async Task ExitMatchmaking()
        {
            await m_matchmakingService.RemovePlayer(m_player);
            m_player.ChangeState(m_playerStateFactory.CreateInMainMenuState(m_player));
            await m_serverService.SendLobbyServerMessageToClient(m_player.ConnectionId, new StopMatchmakingResponseMessage(true, "OK"));
        }

        public override Task JoinLobby(string code)
        {
            throw new NotSupportedException("Cannot execute action in this state!");
        }

        public override Task LeaveLobby()
        {
            throw new NotSupportedException("Cannot execute action in this state!");
        }

        public override async Task StartGame()
        {
            if (m_matchmakingService.TryGetMatch(m_player, out Match? match) && match is not null)
            {
                string code = match.GameCode;
                IPlayerClient otherPlayer = match.Client;
                m_player.ChangeState(m_playerStateFactory.CreateInGameState(m_player, code));
                otherPlayer.ChangeState(m_playerStateFactory.CreateInGameState(otherPlayer, code));
                await m_serverService.LeaveGroup(m_player.ConnectionId, nameof(InMainMenu));
                await m_serverService.LeaveGroup(otherPlayer.ConnectionId, nameof(InMainMenu));
                await m_serverService.JoinGroup(m_player.ConnectionId, code);
                await m_serverService.JoinGroup(otherPlayer.ConnectionId, code);
                if (m_gameManager.AddGame(code, out IGame? game) &&
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
                    game.Context.EventManager.Subscribe<OnGameEnded>(GameEnded);
                    game.Context.EventManager.Subscribe<MilitaryVictory>(OnMilitaryVictory);
                    game.Context.EventManager.Subscribe<ScientificVictory>(OnScientificVictory);
                    m_gameManager.StartGame(code);
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
        }

        public override Task StartMatchmaking()
        {
            throw new NotSupportedException("Cannot execute action in this state!");
        }

        private void OnMilitaryVictory(MilitaryVictory victory)
        {
            RecordCompetitiveWinner(victory.PlayerProperties.Owner).GetAwaiter().GetResult();
        }

        private void OnScientificVictory(ScientificVictory victory)
        {
            RecordCompetitiveWinner(victory.PlayerProperties.Owner).GetAwaiter().GetResult();
        }

        private void GameEnded(OnGameEnded ended)
        {
            Player? winner = null;
            if (ended.FirstPlayer.VictoryPoints > ended.SecondPlayer.VictoryPoints)
            {
                winner = ended.FirstPlayer.Owner;
            }
            else if (ended.SecondPlayer.VictoryPoints > ended.FirstPlayer.VictoryPoints)
            {
                winner = ended.SecondPlayer.Owner;
            }
            else
            {
                if (ended.FirstPlayer.Owner.Cards.OfType<BlueCard>().Count() > ended.SecondPlayer.Owner.Cards.OfType<BlueCard>().Count())
                {
                    winner = ended.FirstPlayer.Owner;
                }
                else if (ended.SecondPlayer.Owner.Cards.OfType<BlueCard>().Count() > ended.FirstPlayer.Owner.Cards.OfType<BlueCard>().Count())
                {
                    winner = ended.SecondPlayer.Owner;
                }
            }
            if (winner is not null)
            {
                RecordCompetitiveWinner(winner).GetAwaiter().GetResult();
            }
        }

        private async Task RecordCompetitiveWinner(Player winner)
        {
            try
            {
                string winnerUserName = winner.Name;
                using var scope = m_serviceScopeFactory.CreateScope();
                var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                var user = await dbContext.Users.FirstOrDefaultAsync(u => u.UserName == winnerUserName);
                if (user != null)
                {
                    user.CompetitiveWins += 1;
                    await dbContext.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                GameLog.Info($"Failed to record competitive winner: {ex.Message}");
            }
        }

        private readonly IMatchmakingService m_matchmakingService;
        private readonly IGameManager m_gameManager;
        private readonly IRandomGeneratorFactory m_randomGeneratorFactory;
        private readonly IServiceScopeFactory m_serviceScopeFactory;
    }
}
