using GameLogic.Elements;
using GameLogic.Elements.Wonders;
using SevenWonders.Presenter.Views;
using SevenWonders.Presenter.Views.Factories;

namespace SevenWonders.Presenter.Connectors
{
    public class WonderConnector : IWonderConnector
    {
        public WonderConnector(IGameElements gameElements, IGameObjectViewFactory gameObjectViewFactory)
        {
            m_wonderList = gameElements.Wonders;
            m_gameObjectViewFactory = gameObjectViewFactory;
        }

        

        public IDictionary<Wonder, IGameObjectView> ReceiveWonderConnection()
        {
            Dictionary<Wonder, IGameObjectView> result = new Dictionary<Wonder, IGameObjectView>();
            foreach (Wonder wonder in m_wonderList.Wonders)
            {
                result.Add(wonder, m_gameObjectViewFactory.CreateView(wonder.Name));
            }

            return result;
        }

        private readonly IGameObjectViewFactory m_gameObjectViewFactory;
        private readonly IWonderList m_wonderList;
    }
}
