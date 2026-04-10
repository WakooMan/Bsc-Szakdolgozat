using SevenWonders.GameEngine;
using SevenWonders.Presenter.Presenters.Handlers;

namespace SevenWonders.Presenter.Presenters.Factories
{
    public interface IDevelopmentHandlerFactory
    {
        IDevelopmentHandler Create(GraphicsLayer graphicsLayer, GameObject developmentTarget);
    }
}
