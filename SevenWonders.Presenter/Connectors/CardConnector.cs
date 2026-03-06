using GameLogic.Elements;
using GameLogic.Elements.GameCards;
using SevenWonders.Presenter.Views;
using SevenWonders.Presenter.Views.Factories;

namespace SevenWonders.Presenter.Connectors
{
    public class CardConnector : ICardConnector
    {
        public CardConnector(IGameElements gameElements, IGameObjectViewFactory gameObjectViewFactory)
        {
            m_cardList = gameElements.Cards;
            m_gameObjectViewFactory = gameObjectViewFactory;
        }

        public IDictionary<Card, IGameObjectView> ReceiveCardConnection()
        {
            Dictionary<Card, IGameObjectView> result = new Dictionary<Card, IGameObjectView>();
            foreach (Card card in m_cardList.Cards)
            {
                result.Add(card, m_gameObjectViewFactory.CreateView(card.Name));
            }

            return result;
        }

        private readonly IGameObjectViewFactory m_gameObjectViewFactory;
        private readonly ICardList m_cardList;
    }
}
