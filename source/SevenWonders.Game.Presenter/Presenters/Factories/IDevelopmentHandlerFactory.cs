using SevenWonders.Game.Engine.SceneHandling;
using SevenWonders.Game.Engine.SceneObjects;
using SevenWonders.Game.Presenter.Presenters.Handlers;

namespace SevenWonders.Game.Presenter.Presenters.Factories
{
    public interface IDevelopmentHandlerFactory
    {
        IDevelopmentHandler Create(GraphicsLayer graphicsLayer, GameObject developmentTarget);
    }
}
