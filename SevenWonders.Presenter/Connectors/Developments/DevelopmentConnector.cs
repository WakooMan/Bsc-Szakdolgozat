using GameLogic.Elements;
using GameLogic.Elements.Developments;
using GameLogic.Elements.Modifiers;
using GameLogic.Elements.Wonders;
using SevenWonders.GameEngine;
using SevenWonders.Presenter.Connectors.Wonders;
using SevenWonders.Presenter.Views;
using SevenWonders.Presenter.Views.Factories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SevenWonders.Presenter.Connectors.Developments
{
    public class DevelopmentConnector : IDevelopmentConnector
    {
        public DevelopmentConnector(IGameElements gameElements, IGameObjectViewFactory gameObjectViewFactory)
        {
            m_developmentList = gameElements.Developments;
            m_gameObjectViewFactory = gameObjectViewFactory;
        }
        public IDictionary<Development, IGameObjectView> ReceiveDevelopmentConnection()
        {
            Dictionary<Development, IGameObjectView> result = new Dictionary<Development, IGameObjectView>();
            foreach (Development development in m_developmentList.Developments)
            {
                result.Add(development, m_gameObjectViewFactory.CreateView(development.Name));
            }

            return result;
        }

        private readonly IDevelopmentList m_developmentList;
        private readonly IGameObjectViewFactory m_gameObjectViewFactory;
    }
}
