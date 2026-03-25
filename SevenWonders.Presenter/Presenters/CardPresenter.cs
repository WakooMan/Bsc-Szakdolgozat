using GameLogic.Ages;
using GameLogic.Elements;
using GameLogic.Elements.GameCards;
using SevenWonders.GameEngine;
using SevenWonders.Presenter.Connectors;
using SevenWonders.Presenter.Connectors.Cards;
using SevenWonders.Presenter.Views;
using SkiaSharp.Views.Maui;
using System.Numerics;
using static SevenWonders.Presenter.Presenters.ICardPresenter;

namespace SevenWonders.Presenter.Presenters
{
    public class CardPresenter : ICardPresenter
    {
        public event CardPresenterDelegate? CardChosen;
        public event DecisionDelegate? BuildCardChosen;
        public event DecisionDelegate? SellCardChosen;
        public event DecisionDelegate? UnpickCardChosen;
        public event DecisionDelegate? BuildWonderChosen;

        public CardPresenter(ICardConnector cardConnector, IGameEngineReceiver gameEngineReceiver)
        {
            m_cardConnector = cardConnector;
            m_gameEngineReceiver = gameEngineReceiver;
            m_cards = new Dictionary<Card, IGameObjectView>();
            m_ageCardDecks = new Dictionary<AgesEnum, GameObject>();
            m_player1Targets = new List<GameObject>();
            m_player2Targets = new List<GameObject>();
            m_centerTargets = new Dictionary<string, GameObject>();
        }

        public void Initialize()
        {
            m_cardActionLocation = m_gameEngineReceiver.ReceiveGameObject("CardActionLocation");
            m_pickCardLayer = m_gameEngineReceiver.ReceiveGraphicsLayer("PickCardLayer");
            m_firstAgeLayer = m_gameEngineReceiver.ReceiveGraphicsLayer("FirstAge");
            m_secondAgeLayer = m_gameEngineReceiver.ReceiveGraphicsLayer("SecondAge");
            m_thirdAgeLayer = m_gameEngineReceiver.ReceiveGraphicsLayer("ThirdAge");
            m_dropCardDeck = m_gameEngineReceiver.ReceiveGameObject("DropCardDeck");
            m_ageCardDecks[AgesEnum.I] = m_gameEngineReceiver.ReceiveGameObject("FirstAgeDeck");
            m_ageCardDecks[AgesEnum.II] = m_gameEngineReceiver.ReceiveGameObject("SecondAgeDeck");
            m_ageCardDecks[AgesEnum.III] = m_gameEngineReceiver.ReceiveGameObject("ThirdAgeDeck");
            m_buildWonderButton = m_gameEngineReceiver.ReceiveButton("BuildWonder");
            m_unpickCardButton = m_gameEngineReceiver.ReceiveButton("UnpickCard");
            m_buildCardButton = m_gameEngineReceiver.ReceiveButton("BuildCard");
            m_sellCardButton = m_gameEngineReceiver.ReceiveButton("SellCard");

            foreach (var connection in m_cardConnector.ReceiveCardConnection())
            {
                m_cards[connection.Key] = connection.Value;
                connection.Value.GetAnimationGroupBuilder().Flip(0, 0f).MoveTo(m_ageCardDecks[connection.Key.Age], 0f);
                connection.Value.Execute();
            }

            foreach (var firstAgeCenterTarget in m_gameEngineReceiver.ReceiveGameObjects("firstAgeCenter", 20))
            {
                m_centerTargets[firstAgeCenterTarget.Name] = firstAgeCenterTarget;
            }

            foreach (var secondAgeCenterTarget in m_gameEngineReceiver.ReceiveGameObjects("secondAgeCenter", 20))
            {
                m_centerTargets[secondAgeCenterTarget.Name] = secondAgeCenterTarget;
            }

            foreach (var thirdAgeCenterTarget in m_gameEngineReceiver.ReceiveGameObjects("thirdAgeCenter", 20))
            {
                m_centerTargets[thirdAgeCenterTarget.Name] = thirdAgeCenterTarget;
            }

            foreach (var player1CardTarget in m_gameEngineReceiver.ReceiveGameObjects("player1Card", 1))
            {
                m_player1Targets.Add(player1CardTarget);
            }

            foreach (var player2CardTarget in m_gameEngineReceiver.ReceiveGameObjects("player2Card", 1))
            {
                m_player2Targets.Add(player2CardTarget);
            }

            m_firstAgeLayer.Visible = true;
            m_secondAgeLayer.Visible = true;
            m_thirdAgeLayer.Visible = true;
        }

        public void MoveToActionLocation(Card card)
        {
            if (m_cardActionLocation is not null)
            {
                var view = m_cards[card];
                var group = view.GetAnimationGroupBuilder()
                .MoveTo(m_cardActionLocation, 0.3f)
                .Highlight(m_cardActionLocation.VisualSize, false, 0.3f);
                view.Execute();
                m_pickCardLayer.Visible = true;
                m_buildWonderButton.ClickedEvent += OnBuildWonderButtonClicked;
                m_unpickCardButton.ClickedEvent += OnUnpickCardButtonClicked;
                m_buildCardButton.ClickedEvent += OnBuildCardButtonClicked;
                m_sellCardButton.ClickedEvent += OnSellCardButtonClicked;
            }
        }

        private void UnsubscribeDecisionButtons()
        {
            m_buildWonderButton.ClickedEvent -= OnBuildWonderButtonClicked;
            m_unpickCardButton.ClickedEvent -= OnUnpickCardButtonClicked;
            m_buildCardButton.ClickedEvent -= OnBuildCardButtonClicked;
            m_sellCardButton.ClickedEvent -= OnSellCardButtonClicked;
            m_pickCardLayer.Visible = false;
        }

        private void OnBuildWonderButtonClicked(SKTouchEventArgs eventArgs)
        {
            UnsubscribeDecisionButtons();
            BuildWonderChosen?.Invoke();
        }

        private void OnUnpickCardButtonClicked(SKTouchEventArgs eventArgs)
        {
            UnsubscribeDecisionButtons();
            UnpickCardChosen?.Invoke();
        }

        private void OnBuildCardButtonClicked(SKTouchEventArgs eventArgs)
        {
            UnsubscribeDecisionButtons();
            BuildCardChosen?.Invoke();
        }

        private void OnSellCardButtonClicked(SKTouchEventArgs eventArgs)
        {
            UnsubscribeDecisionButtons();
            SellCardChosen?.Invoke();
        }

        public void MoveToCenter(Card card, bool hidden, string nodeName)
        {
            var view = m_cards[card];
            var group = view.GetAnimationGroupBuilder();
            if (m_centerTargets.TryGetValue(nodeName, out var target))
            {
                group.MoveTo(target, 0.5f);
            }
            if (!hidden)
            {
                group.Flip(1, 0.5f);
            }
            view.Execute();
            view.IncreaseZIndex();
            view.SubscribeClickAtAnimationEnd(() => CardChosen?.Invoke(card));
        }

        public void MoveBackToCenter(Card card, string nodeName)
        {
            var view = m_cards[card];
            var group = view.GetAnimationGroupBuilder();
            if (m_centerTargets.TryGetValue(nodeName, out var target))
            {
                group.MoveTo(target, 0.5f)
                .Unhighlight(false, 0.5f);
            }

            view.Execute();
            view.DecreaseZIndex();
            //view.SubscribeClickAtAnimationEnd(() => CardChosen?.Invoke(card));
        }

        public void MoveToDropCardDeck(Card card)
        {
            if (m_dropCardDeck is not null)
            {
                var view = m_cards[card];
                var group = view.GetAnimationGroupBuilder();
                group.MoveTo(m_dropCardDeck, 1.5f);
                view.Execute();
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
        private readonly IGameEngineReceiver m_gameEngineReceiver;
        private readonly List<GameObject> m_player1Targets;
        private readonly List<GameObject> m_player2Targets;
        private GraphicsLayer? m_pickCardLayer;
        private GraphicsLayer? m_firstAgeLayer;
        private GraphicsLayer? m_secondAgeLayer;
        private GraphicsLayer? m_thirdAgeLayer;
        private readonly Dictionary<string, GameObject> m_centerTargets;
        private GameObject? m_cardActionLocation;
        private ButtonObject m_buildWonderButton;
        private ButtonObject m_unpickCardButton;
        private ButtonObject m_buildCardButton;
        private ButtonObject m_sellCardButton;
        private readonly Dictionary<AgesEnum , GameObject> m_ageCardDecks;
        private GameObject? m_dropCardDeck;
    }
}
