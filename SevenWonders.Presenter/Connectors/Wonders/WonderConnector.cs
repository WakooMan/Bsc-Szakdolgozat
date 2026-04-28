using GameLogic.Elements;
using GameLogic.Elements.Wonders;
using SevenWonders.Presenter.Connectors.Wonders.WonderChildTextureHandlers;
using SevenWonders.Presenter.Views.Factories;

namespace SevenWonders.Presenter.Connectors.Wonders
{
    public class WonderConnector : IWonderConnector
    {
        public WonderConnector(IGameElements gameElements, IGameObjectViewFactory gameObjectViewFactory, IWonderChildTextureHandler wonderChildTextureHandler)
        {
            m_gameElements = gameElements;
            m_gameObjectViewFactory = gameObjectViewFactory;
            m_wonderChildTextureHandler = wonderChildTextureHandler;
        }

        

        public IDictionary<Wonder, WonderConnection> ReceiveWonderConnection()
        {
            IWonderList? wonderList = m_gameElements.Wonders;
            Dictionary<Wonder, WonderConnection> result = new Dictionary<Wonder, WonderConnection>();
            if(wonderList is null)
            {
                return result;
            }

            foreach (Wonder wonder in wonderList.Wonders)
            {
                result.Add(wonder, new WonderConnection(m_gameObjectViewFactory.CreateView(wonder.Name)));
                m_wonderChildTextureHandler.Handle(wonder);
            }

            return result;
        }

        private readonly IGameObjectViewFactory m_gameObjectViewFactory;
        private readonly IGameElements m_gameElements;
        private readonly IWonderChildTextureHandler m_wonderChildTextureHandler;
    }
}
