using GameLogic;
using WebServer.Model.Client;

namespace WebServer.Model
{
    public interface IGameInitializer
    {
        IGame CreateAndInitialize(IPlayerClient player1, IPlayerClient player2);
    }
}
