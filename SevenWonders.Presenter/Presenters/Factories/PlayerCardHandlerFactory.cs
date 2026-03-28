using SevenWonders.GameEngine;
using SevenWonders.Presenter.Presenters.Handlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SevenWonders.Presenter.Presenters.Factories
{
    public class PlayerCardHandlerFactory : IPlayerCardHandlerFactory
    {
        public PlayerCardHandlerFactory(IObjectManager objectManager)
        {
            m_objectManager = objectManager;
        }

        public IPlayerCardHandler Create(Scene scene, GraphicsLayer graphicsLayer, GameObject cardTarget)
        {
            return new PlayerCardHandler(m_objectManager, scene, graphicsLayer, cardTarget);
        }

        private readonly IObjectManager m_objectManager;
    }
}
