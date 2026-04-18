using GameLogic;

namespace WebServer.Model
{
    public interface IGameFactory
    {
        IGame Create();
    }
}
