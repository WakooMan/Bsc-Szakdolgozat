using GameLogic.Ages;
using GameLogic.Elements;
using GameLogic.Elements.GameCards;
using SevenWonders.GameEngine;
using SevenWonders.Presenter.Connectors;
using SevenWonders.Presenter.Views;

namespace SevenWonders.Presenter.Presenters
{
    public class CardPresenter : ICardPresenter
    {
        public CardPresenter(ICardConnector cardConnector, IGameObjectReceiver gameObjectReceiver)
        {
            m_cardConnector = cardConnector;
            m_gameObjectReceiver = gameObjectReceiver;
            m_cards = new Dictionary<Card, IGameObjectView>();
            m_player1Targets = new List<GameObject>();
            m_player2Targets = new List<GameObject>();
            m_firstAgeCenterTargets = new Stack<GameObject>();
            m_secondAgeCenterTargets = new Stack<GameObject>();
            m_thirdAgeCenterTargets = new Stack<GameObject>();
        }

        public void Initialize()
        {
            m_cardActionLocation = m_gameObjectReceiver.ReceiveGameObject("CardActionLocation");
            m_DropCardDeck = m_gameObjectReceiver.ReceiveGameObject("DropCardDeck");

            foreach (var connection in m_cardConnector.ReceiveCardConnection())
            {
                m_cards[connection.Key] = connection.Value;
            }

            foreach (var firstAgeCenterTarget in m_gameObjectReceiver.ReceiveGameObjects("firstAgeCenter", 20))
            {
                m_firstAgeCenterTargets.Push(firstAgeCenterTarget);
            }

            foreach (var secondAgeCenterTarget in m_gameObjectReceiver.ReceiveGameObjects("secondAgeCenter", 20))
            {
                m_secondAgeCenterTargets.Push(secondAgeCenterTarget);
            }

            foreach (var thirdAgeCenterTarget in m_gameObjectReceiver.ReceiveGameObjects("thirdAgeCenter", 20))
            {
                m_thirdAgeCenterTargets.Push(thirdAgeCenterTarget);
            }

            foreach (var player1CardTarget in m_gameObjectReceiver.ReceiveGameObjects("player1Card", 1))
            {
                m_player1Targets.Add(player1CardTarget);
            }

            foreach (var player2CardTarget in m_gameObjectReceiver.ReceiveGameObjects("player2Card", 1))
            {
                m_player2Targets.Add(player2CardTarget);
            }

        }

        public void MoveToActionLocation(Card card)
        {
            if (m_cardActionLocation is not null)
            {
                m_cards[card].MoveTo(m_cardActionLocation);
            }
        }

        public void MoveToCenter(Card card, AgesEnum age)
        {
            switch (age)
            {
                case AgesEnum.I:
                    m_cards[card].MoveTo(m_firstAgeCenterTargets.Pop());
                    break;
                case AgesEnum.II:
                    m_cards[card].MoveTo(m_secondAgeCenterTargets.Pop());
                    break;
                case AgesEnum.III:
                    m_cards[card].MoveTo(m_thirdAgeCenterTargets.Pop());
                    break;
            }
        }

        public void MoveToDropCardDeck(Card card)
        {
            if (m_DropCardDeck is not null)
            {
                m_cards[card].MoveTo(m_DropCardDeck);
            }
        }

        public void MoveToPlayer(Player player, Card card)
        {
            if (player.Id == 1)
            {
                MoveToPlayer1(card);
            }
            if (player.Id == 2)
            {
                MoveToPlayer2(card);
            }
        }

        private void MoveToPlayer1(Card card)
        {
            //if (m_player1Targets.ContainsKey(card.GetType()))
            //{
            //    m_cards[card].MoveTo(m_player1Targets[card.GetType()]);
            //}
        }

        private void MoveToPlayer2(Card card)
        {
            //if (m_player2Targets.ContainsKey(card.GetType()))
            //{
            //    m_cards[card].MoveTo(m_player2Targets[card.GetType()]);
            //}
        }

        private readonly IDictionary<Card, IGameObjectView> m_cards;
        private readonly ICardConnector m_cardConnector;
        private readonly IGameObjectReceiver m_gameObjectReceiver;
        private readonly List<GameObject> m_player1Targets;
        private readonly List<GameObject> m_player2Targets;
        private readonly Stack<GameObject> m_firstAgeCenterTargets;
        private readonly Stack<GameObject> m_secondAgeCenterTargets;
        private readonly Stack<GameObject> m_thirdAgeCenterTargets;
        private GameObject? m_cardActionLocation;
        //private GameObject? m_firstAgeCardDeck;
        //private GameObject? m_secondAgeCardDeck;
        //private GameObject? m_thirdAgeCardDeck;
        //private GameObject? m_SpecialCardDeck;
        private GameObject? m_DropCardDeck;
    }
}
