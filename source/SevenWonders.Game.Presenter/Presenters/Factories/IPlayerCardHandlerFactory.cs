using SevenWonders.Game.Engine;
using SevenWonders.Game.Presenter.Presenters.Handlers;

namespace SevenWonders.Game.Presenter.Presenters.Factories
{
    public interface IPlayerCardHandlerFactory
    {
        IPlayerCardHandler Create(Scene scene, GraphicsLayer graphicsLayer, GameObject cardTarget);
    }
}
