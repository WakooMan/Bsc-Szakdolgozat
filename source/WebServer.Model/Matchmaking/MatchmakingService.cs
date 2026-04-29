using System.Collections.Concurrent;
using WebServer.Model.Client;
using WebServer.Model.Lobby;

namespace WebServer.Model.Matchmaking
{
    public class MatchmakingService : IMatchmakingService
    {
        public MatchmakingService(ILobbyCodeGenerator lobbyCodeGenerator)
        {
            m_lobbyCodeGenerator = lobbyCodeGenerator;
        }

        public async Task AddPlayer(IPlayerClient player)
        {
            m_queue.TryAdd(player.ConnectionId, player);
            await TryMatch();
        }

        public Task RemovePlayer(IPlayerClient player)
        {
            m_queue.TryRemove(player.ConnectionId, out _);
            return Task.CompletedTask;
        }

        private async Task TryMatch()
        {
            await m_matchLock.WaitAsync();
            try
            {
                if (m_queue.Count < 2)
                    return;

                IPlayerClient? player1 = null;
                IPlayerClient? player2 = null;

                foreach (var kvp in m_queue)
                {
                    if (player1 == null)
                    {
                        player1 = kvp.Value;
                    }
                    else
                    {
                        player2 = kvp.Value;
                        break;
                    }
                }

                if (player1 == null || player2 == null)
                    return;

                m_queue.TryRemove(player1.ConnectionId, out _);
                m_queue.TryRemove(player2.ConnectionId, out _);

                await StartCompetitiveGame(player1, player2);
            }
            finally
            {
                m_matchLock.Release();
            }
        }

        private async Task StartCompetitiveGame(IPlayerClient player1, IPlayerClient player2)
        {
            string code = m_lobbyCodeGenerator.GenerateUniqueCode();
            if (m_matches.TryAdd(player1, new Match(player2, code)))
            {
                await player1.StartGame();
            }
        }

        public bool TryGetMatch(IPlayerClient player, out Match? match)
        {
            return m_matches.TryGetValue(player, out match);
        }

        private readonly ConcurrentDictionary<string, IPlayerClient> m_queue = new();
        private readonly ConcurrentDictionary<IPlayerClient, Match> m_matches = new();
        private readonly SemaphoreSlim m_matchLock = new(1, 1);
        private readonly ILobbyCodeGenerator m_lobbyCodeGenerator;
    }
}
