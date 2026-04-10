using GameLogic.Elements;
using GameLogic.Elements.GameCards;
using SevenWonders.Presenter.Connectors.Cards.CardChildTextureHandlers;
using SevenWonders.Presenter.Views;
using SevenWonders.Presenter.Views.Factories;

namespace SevenWonders.Presenter.Connectors.Cards
{
    public class CardConnector : ICardConnector
    {
        public CardConnector(IGameElements gameElements, IGameObjectViewFactory gameObjectViewFactory, ICardChildTextureHandler cardChildTextureHandler)
        {
            m_cardList = gameElements.Cards;
            m_gameObjectViewFactory = gameObjectViewFactory;
            m_cardChildTextureHandler = cardChildTextureHandler;
        }

        public IDictionary<Card, IGameObjectView> ReceiveCardConnection()
        {
            Dictionary<Card, IGameObjectView> result = new Dictionary<Card, IGameObjectView>();
            foreach (Card card in m_cardList.Cards)
            {
                IGameObjectView view = m_gameObjectViewFactory.CreateView(card.Name);
                m_cardChildTextureHandler.Handle(card);
                result.Add(card, view);
            }

            return result;
        }

        private readonly IGameObjectViewFactory m_gameObjectViewFactory;
        private readonly ICardChildTextureHandler m_cardChildTextureHandler;
        private readonly ICardList m_cardList;
    }
}
