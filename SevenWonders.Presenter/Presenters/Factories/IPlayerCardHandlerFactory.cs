using SevenWonders.GameEngine;
using SevenWonders.Presenter.Presenters.Handlers;

namespace SevenWonders.Presenter.Presenters.Factories
{
    public interface IPlayerCardHandlerFactory
    {
        IPlayerCardHandler Create(Scene scene, GraphicsLayer graphicsLayer, GameObject cardTarget);
    }
}
