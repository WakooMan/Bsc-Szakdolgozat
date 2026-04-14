using GameLogic;
using WebServer.Model.Client;

namespace WebServer.Model
{
    public interface IGameManager
    {
        IGame? GetGame(string code);
        bool AddGame(IPlayerClient player1, IPlayerClient player2, string code, out IGame? game);
        bool RemoveGame(string code);
        void StartGame(string code);
    }
}
