using SevenWonders.Game.Logic;
using System.Collections.Concurrent;

namespace SevenWonders.Web.Server.Model
{
    public class GameManager : IGameManager
    {
        public GameManager(IGameFactory gameInitializer)
        {
            m_gameInitializer = gameInitializer;
            Games = new ConcurrentDictionary<string, IGame>();
        }

        public ConcurrentDictionary<string, IGame> Games { get; }
        public bool AddGame(string code, out IGame? game)
        {
            game = m_gameInitializer.Create();
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

        private readonly IGameFactory m_gameInitializer;
    }
}
