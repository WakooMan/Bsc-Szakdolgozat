using GameLogic;
using System.Collections.Concurrent;
using WebServer.Model.Client;
using WebServer.Model.PlayerStates;

namespace WebServer.Model
{
    public class GameManager : IGameManager
    {
        public GameManager(IGameInitializer gameInitializer)
        {
            m_gameInitializer = gameInitializer;
            Games = new ConcurrentDictionary<string, IGame>();
        }

        public ConcurrentDictionary<string, IGame> Games { get; }
        public bool AddGame(IPlayerClient player1, IPlayerClient player2, string code, out IGame game)
        {
            game = m_gameInitializer.CreateAndInitialize(player1, player2);
            return Games.TryAdd(code, game);
        }

        public IGame? GetGame(string code)
        {
            if (Games.TryGetValue(code, out IGame? game))
            {
                return game;
            }
            return null;
        }

        public bool RemoveGame(string code)
        {
            return Games.TryRemove(code, out _);
        }

        public void StartGame(string code)
        {
            if (Games.TryGetValue(code, out IGame? game))
            {
                _ = Task.Run(game.GameLoop);
            }
        }

        private readonly IGameInitializer m_gameInitializer;
    }
}
