using SevenWonders.Game.Logic;

namespace SevenWonders.Web.Server.Model
{
    public interface IGameFactory
    {
        IGame Create();
    }
}
