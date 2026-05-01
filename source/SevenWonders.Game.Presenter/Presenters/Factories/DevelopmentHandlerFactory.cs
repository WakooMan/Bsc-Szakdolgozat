using SevenWonders.Game.Engine;
using SevenWonders.Game.Presenter.Presenters.Handlers;

namespace SevenWonders.Game.Presenter.Presenters.Factories
{
    public class DevelopmentHandlerFactory : IDevelopmentHandlerFactory
    {
        public DevelopmentHandlerFactory(IObjectManager objectManager)
        {
            m_objectManager = objectManager;
        }

        public IDevelopmentHandler Create(GraphicsLayer graphicsLayer, GameObject developmentTarget)
        {
            return new DevelopmentHandler(m_objectManager, graphicsLayer, developmentTarget);
        }

        private readonly IObjectManager m_objectManager;
    }
}
