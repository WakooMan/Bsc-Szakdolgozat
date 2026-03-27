using GameLogic.Ages;
using GameLogic.Elements;
using GameLogic.Elements.GameCards;
using GameLogic.Events;
using GameLogic.Events.GameEvents;
using GameLogic.GameStructures;
using SevenWonders.GameEngine;
using SevenWonders.Presenter.Connectors;
using SevenWonders.Presenter.Connectors.Cards;
using SevenWonders.Presenter.Views;

namespace SevenWonders.Presenter.Presenters
{
    public class CardPresenter : IPresenter
    {

        public CardPresenter(ICardConnector cardConnector, IGameEngineReceiver gameEngineReceiver, IEventManager eventManager)
        {
            m_cardConnector = cardConnector;
            m_gameEngineReceiver = gameEngineReceiver;
            m_eventManager = eventManager;
            m_cards = new Dictionary<Card, IGameObjectView>();
            m_ageCardDecks = new Dictionary<AgesEnum, GameObject>();
            m_player1Targets = new Dictionary<Type, GameObject>();
            m_player2Targets = new Dictionary<Type, GameObject>();
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

            foreach (var connection in m_cardConnector.ReceiveCardConnection())
            {
                m_cards[connection.Key] = connection.Value;
                connection.Value.GetAnimationGroupBuilder().Flip(0, 0f).MoveTo(m_ageCardDecks[connection.Key.Age], 0f);
                _ = connection.Value.Execute();
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

            m_player1Targets.Add(typeof(RedCard), m_gameEngineReceiver.ReceiveGameObject("player1RedCard"));
            m_player1Targets.Add(typeof(GreenCard), m_gameEngineReceiver.ReceiveGameObject("player1GreenCard"));
            m_player1Targets.Add(typeof(GrayCard), m_gameEngineReceiver.ReceiveGameObject("player1GrayCard"));
            m_player1Targets.Add(typeof(BrownCard), m_gameEngineReceiver.ReceiveGameObject("player1BrownCard"));
            m_player1Targets.Add(typeof(PurpleCard), m_gameEngineReceiver.ReceiveGameObject("player1PurpleCard"));
            m_player1Targets.Add(typeof(YellowCard), m_gameEngineReceiver.ReceiveGameObject("player1YellowCard"));
            m_player1Targets.Add(typeof(BlueCard), m_gameEngineReceiver.ReceiveGameObject("player1BlueCard"));


            m_player2Targets.Add(typeof(RedCard), m_gameEngineReceiver.ReceiveGameObject("player2RedCard"));
            m_player2Targets.Add(typeof(GreenCard), m_gameEngineReceiver.ReceiveGameObject("player2GreenCard"));
            m_player2Targets.Add(typeof(GrayCard), m_gameEngineReceiver.ReceiveGameObject("player2GrayCard"));
            m_player2Targets.Add(typeof(BrownCard), m_gameEngineReceiver.ReceiveGameObject("player2BrownCard"));
            m_player2Targets.Add(typeof(PurpleCard), m_gameEngineReceiver.ReceiveGameObject("player2PurpleCard"));
            m_player2Targets.Add(typeof(YellowCard), m_gameEngineReceiver.ReceiveGameObject("player2YellowCard"));
            m_player2Targets.Add(typeof(BlueCard), m_gameEngineReceiver.ReceiveGameObject("player2BlueCard"));

            m_firstAgeLayer.Visible = true;
            m_secondAgeLayer.Visible = true;
            m_thirdAgeLayer.Visible = true;
        }

        public void SubscribeToEvents()
        {
            m_eventManager.Subscribe<OnAgeStarted>(state => {
                foreach (ICardNode cardNode in state.Age.Composition.AllCards)
                {
                    MoveToCenter(cardNode.CardObj, cardNode.Hidden, cardNode.NodeName).GetAwaiter().GetResult();
                }
            });

            m_eventManager.Subscribe<OnCardPicked>(eventObj => {
                MoveToActionLocation(eventObj.Card).GetAwaiter().GetResult();
            });
            m_eventManager.Subscribe<OnCardUnpicked>(eventObj => {
                if (m_pickCardLayer is not null)
                {
                    m_pickCardLayer.Visible = false;
                }
                MoveBackToCenter(eventObj.CardNode.CardObj, eventObj.CardNode.NodeName).GetAwaiter().GetResult();
            });
            m_eventManager.Subscribe<OnCardBuilt>(eventObj => {
                if (m_pickCardLayer is not null)
                {
                    m_pickCardLayer.Visible = false;
                }
                MoveToPlayer(eventObj.Builder, eventObj.Card).GetAwaiter().GetResult();
            });
            m_eventManager.Subscribe<OnCardSold>(eventObj => {
                if (m_pickCardLayer is not null)
                {
                    m_pickCardLayer.Visible = false;
                }
                MoveToDropCardDeck(eventObj.Card).GetAwaiter().GetResult();
            });
        }

        private async Task MoveToActionLocation(Card card)
        {
            if (m_cardActionLocation is not null)
            {
                var view = m_cards[card];
                view.GetAnimationGroupBuilder()
                    .MoveTo(m_cardActionLocation, 0.3f)
                    .Highlight(m_cardActionLocation.VisualSize, false, 0.3f);
                await view.Execute();
                if (m_pickCardLayer is not null)
                {
                    m_pickCardLayer.Visible = true;
                }
            }
        }

        private async Task MoveToCenter(Card card, bool hidden, string nodeName)
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
            await view.Execute();
            view.IncreaseZIndex();
        }

        private async Task MoveBackToCenter(Card card, string nodeName)
        {
            var view = m_cards[card];
            var group = view.GetAnimationGroupBuilder();
            if (m_centerTargets.TryGetValue(nodeName, out var target))
            {
                group.MoveTo(target, 0.5f)
                    .Unhighlight(false, 0.5f);
            }

            await view.Execute();
            view.DecreaseZIndex();
        }

        private async Task MoveToDropCardDeck(Card card)
        {
            if (m_dropCardDeck is not null)
            {
                var view = m_cards[card];
                var animationBuilder = view.GetAnimationGroupBuilder().Unhighlight(false, 0.1f);
                await view.Execute();
                animationBuilder.MoveTo(m_dropCardDeck, 0.5f).Flip(0, 0.5f);
                await view.Execute();
            }
        }

        private async Task MoveToPlayer(Player player, Card card)
        {
            if (player.Id == 1)
            {
                await MoveToPlayer1(card);
            }
            if (player.Id == 2)
            {
                await MoveToPlayer2(card);
            }
        }

        private async Task MoveToPlayer1(Card card)
        {
            if (m_player1Targets.ContainsKey(card.GetType()))
            {
                var view = m_cards[card];
                view.GetAnimationGroupBuilder()
                    .MoveTo(m_player1Targets[card.GetType()], 0.5f)
                    .Unhighlight(false, 0.5f);
                await view.Execute();
            }
        }

        private async Task MoveToPlayer2(Card card)
        {
            if (m_player2Targets.ContainsKey(card.GetType()))
            {
                var view = m_cards[card];
                view.GetAnimationGroupBuilder()
                    .MoveTo(m_player2Targets[card.GetType()], 0.5f)
                    .Unhighlight(false, 0.5f);
                await view.Execute();
            }
        }

        private readonly IDictionary<Card, IGameObjectView> m_cards;
        private readonly ICardConnector m_cardConnector;
        private readonly IGameEngineReceiver m_gameEngineReceiver;
        private readonly IEventManager m_eventManager;
        private readonly Dictionary<Type, GameObject> m_player1Targets;
        private readonly Dictionary<Type, GameObject> m_player2Targets;
        private GraphicsLayer? m_pickCardLayer;
        private GraphicsLayer? m_firstAgeLayer;
        private GraphicsLayer? m_secondAgeLayer;
        private GraphicsLayer? m_thirdAgeLayer;
        private readonly Dictionary<string, GameObject> m_centerTargets;
        private GameObject? m_cardActionLocation;
        private readonly Dictionary<AgesEnum , GameObject> m_ageCardDecks;
        private GameObject? m_dropCardDeck;
    }
}
