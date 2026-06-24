using SevenWonders.Game.Logic;

namespace SevenWonders.Web.Server.Model
{
    public interface IGameManager
    {
        IGame? GetGame(string code);
        bool AddGame(string code, out IGame? game);
        Task<bool> RemoveGame(string code);
        void StartGame(string code);
    }
}
