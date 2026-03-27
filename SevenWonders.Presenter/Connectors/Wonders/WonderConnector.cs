using GameLogic.Elements;
using GameLogic.Elements.Wonders;
using SevenWonders.Presenter.Connectors.Wonders.WonderChildTextureHandlers;
using SevenWonders.Presenter.Views;
using SevenWonders.Presenter.Views.Factories;

namespace SevenWonders.Presenter.Connectors.Wonders
{
    public class WonderConnector : IWonderConnector
    {
        public WonderConnector(IGameElements gameElements, IGameObjectViewFactory gameObjectViewFactory, IWonderChildTextureHandler wonderChildTextureHandler)
        {
            m_wonderList = gameElements.Wonders;
            m_gameObjectViewFactory = gameObjectViewFactory;
            m_wonderChildTextureHandler = wonderChildTextureHandler;
        }

        

        public IDictionary<Wonder, IGameObjectView> ReceiveWonderConnection()
        {
            Dictionary<Wonder, IGameObjectView> result = new Dictionary<Wonder, IGameObjectView>();
            foreach (Wonder wonder in m_wonderList.Wonders)
            {
                result.Add(wonder, m_gameObjectViewFactory.CreateView(wonder.Name));
                m_wonderChildTextureHandler.Handle(wonder);
            }

            return result;
        }

        private readonly IGameObjectViewFactory m_gameObjectViewFactory;
        private readonly IWonderList m_wonderList;
        private readonly IWonderChildTextureHandler m_wonderChildTextureHandler;
    }
}
