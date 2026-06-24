using SevenWonders.Game.Logic;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace SevenWonders.Web.Server.Model
{
    public class GameManager : IGameManager
    {
        public GameManager(IGameFactory gameInitializer)
        {
            m_gameInitializer = gameInitializer;
            Games = new ConcurrentDictionary<string, (IGame, Task?)>();
        }

        public ConcurrentDictionary<string, (IGame, Task?)> Games { get; }
        public bool AddGame(string code, out IGame? game)
        {
            game = m_gameInitializer.Create();
            return Games.TryAdd(code, (game, null));
        }

        public IGame? GetGame(string code)
        {
            if (Games.TryGetValue(code, out (IGame, Task?) pair))
            {
                return pair.Item1;
            }
            return null;
        }

        public async Task<bool> RemoveGame(string code)
        {
            var result = Games.TryRemove(code, out (IGame, Task?) pair);
            pair.Item1?.EndGame();
            if (pair.Item2 is not null)
            {
                await pair.Item2;
            }
            return result;
        }

        public void StartGame(string code)
        {
            if (Games.TryGetValue(code, out (IGame, Task?) pair))
            {
                pair.Item2 = Task.Run(pair.Item1.GameLoop);
            }
        }

        private readonly IGameFactory m_gameInitializer;
    }
}
