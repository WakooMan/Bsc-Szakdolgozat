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
            m_gameElements = gameElements;
            m_gameObjectViewFactory = gameObjectViewFactory;
            m_cardChildTextureHandler = cardChildTextureHandler;
        }

        public IDictionary<Card, IGameObjectView> ReceiveCardConnection()
        {
            ICardList? cardList = m_gameElements.Cards;
            Dictionary<Card, IGameObjectView> result = new Dictionary<Card, IGameObjectView>();
            if(cardList is null)
            {
                return result;
            }

            foreach (Card card in cardList.Cards)
            {
                IGameObjectView view = m_gameObjectViewFactory.CreateView(card.Name);
                m_cardChildTextureHandler.Handle(card);
                result.Add(card, view);
            }

            return result;
        }

        private readonly IGameObjectViewFactory m_gameObjectViewFactory;
        private readonly ICardChildTextureHandler m_cardChildTextureHandler;
        private readonly IGameElements m_gameElements;
    }
}
