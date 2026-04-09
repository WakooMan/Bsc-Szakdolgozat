using SevenWonders.GameEngine;
using SevenWonders.Presenter.Presenters.Handlers;

namespace SevenWonders.Presenter.Presenters.Factories
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
