using SevenWonders.Game.Logic.Ages;
using SevenWonders.Game.Logic.Elements;
using SevenWonders.Game.Logic.Elements.GameCards;
using SevenWonders.Game.Logic.Events;
using SevenWonders.Game.Logic.Events.GameEvents;
using SevenWonders.Game.Logic.GameStructures;
using SevenWonders.Game.Presenter.Connectors;
using SevenWonders.Game.Presenter.Connectors.Cards;
using SevenWonders.Game.Presenter.GameEvents;
using SevenWonders.Game.Presenter.Presenters.Factories;
using SevenWonders.Game.Presenter.Presenters.Handlers;
using SevenWonders.Game.Presenter.Views;
using SevenWonders.Game.Engine.SceneObjects;
using SevenWonders.Game.Engine.SceneHandling;

namespace SevenWonders.Game.Presenter.Presenters
{
    public class CardPresenter : IPresenter
    {

        public CardPresenter(ICardConnector cardConnector, IGameEngineReceiver gameEngineReceiver, IEventManager eventManager, IPlayerCardHandlerFactory playerCardHandlerFactory, ISceneManager sceneManager)
        {
            m_cardConnector = cardConnector;
            m_gameEngineReceiver = gameEngineReceiver;
            m_eventManager = eventManager;
            m_playerCardHandlerFactory = playerCardHandlerFactory;
            m_sceneManager = sceneManager;
            m_cards = new Dictionary<Card, IGameObjectView>();
            m_ageCardDecks = new Dictionary<AgesEnum, GameObject>();
            m_player1Targets = new Dictionary<Type, IPlayerCardHandler>();
            m_player2Targets = new Dictionary<Type, IPlayerCardHandler>();
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
                m_cards.Add(connection);
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

            m_player1Targets.Add(typeof(RedCard),  m_playerCardHandlerFactory.Create(m_sceneManager.CurrentScene, m_gameEngineReceiver.ReceiveGraphicsLayer("background"), m_gameEngineReceiver.ReceiveGameObject("player1RedCard")));
            m_player1Targets.Add(typeof(GreenCard), m_playerCardHandlerFactory.Create(m_sceneManager.CurrentScene,m_gameEngineReceiver.ReceiveGraphicsLayer("background"), m_gameEngineReceiver.ReceiveGameObject("player1GreenCard")));
            m_player1Targets.Add(typeof(GrayCard), m_playerCardHandlerFactory.Create(m_sceneManager.CurrentScene,m_gameEngineReceiver.ReceiveGraphicsLayer("background"), m_gameEngineReceiver.ReceiveGameObject("player1GrayCard")));
            m_player1Targets.Add(typeof(BrownCard), m_playerCardHandlerFactory.Create(m_sceneManager.CurrentScene,m_gameEngineReceiver.ReceiveGraphicsLayer("background"), m_gameEngineReceiver.ReceiveGameObject("player1BrownCard")));
            m_player1Targets.Add(typeof(PurpleCard), m_playerCardHandlerFactory.Create(m_sceneManager.CurrentScene,m_gameEngineReceiver.ReceiveGraphicsLayer("background"), m_gameEngineReceiver.ReceiveGameObject("player1PurpleCard")));
            m_player1Targets.Add(typeof(YellowCard), m_playerCardHandlerFactory.Create(m_sceneManager.CurrentScene,m_gameEngineReceiver.ReceiveGraphicsLayer("background"), m_gameEngineReceiver.ReceiveGameObject("player1YellowCard")));
            m_player1Targets.Add(typeof(BlueCard), m_playerCardHandlerFactory.Create(m_sceneManager.CurrentScene,m_gameEngineReceiver.ReceiveGraphicsLayer("background"), m_gameEngineReceiver.ReceiveGameObject("player1BlueCard")));


            m_player2Targets.Add(typeof(RedCard), m_playerCardHandlerFactory.Create(m_sceneManager.CurrentScene, m_gameEngineReceiver.ReceiveGraphicsLayer("background"), m_gameEngineReceiver.ReceiveGameObject("player2RedCard")));
            m_player2Targets.Add(typeof(GreenCard), m_playerCardHandlerFactory.Create(m_sceneManager.CurrentScene,m_gameEngineReceiver.ReceiveGraphicsLayer("background"), m_gameEngineReceiver.ReceiveGameObject("player2GreenCard")));
            m_player2Targets.Add(typeof(GrayCard), m_playerCardHandlerFactory.Create(m_sceneManager.CurrentScene,m_gameEngineReceiver.ReceiveGraphicsLayer("background"), m_gameEngineReceiver.ReceiveGameObject("player2GrayCard")));
            m_player2Targets.Add(typeof(BrownCard), m_playerCardHandlerFactory.Create(m_sceneManager.CurrentScene,m_gameEngineReceiver.ReceiveGraphicsLayer("background"), m_gameEngineReceiver.ReceiveGameObject("player2BrownCard")));
            m_player2Targets.Add(typeof(PurpleCard), m_playerCardHandlerFactory.Create(m_sceneManager.CurrentScene,m_gameEngineReceiver.ReceiveGraphicsLayer("background"), m_gameEngineReceiver.ReceiveGameObject("player2PurpleCard")));
            m_player2Targets.Add(typeof(YellowCard), m_playerCardHandlerFactory.Create(m_sceneManager.CurrentScene,m_gameEngineReceiver.ReceiveGraphicsLayer("background"), m_gameEngineReceiver.ReceiveGameObject("player2YellowCard")));
            m_player2Targets.Add(typeof(BlueCard), m_playerCardHandlerFactory.Create(m_sceneManager.CurrentScene,m_gameEngineReceiver.ReceiveGraphicsLayer("background"), m_gameEngineReceiver.ReceiveGameObject("player2BlueCard")));

            m_firstAgeLayer.Visible = true;
            m_secondAgeLayer.Visible = true;
            m_thirdAgeLayer.Visible = true;
        }

        public void SubscribeToEvents()
        {
            m_eventManager.Subscribe<OnGameInitialized>(eventObj =>
            {
                foreach (var connection in m_cards)
                {
                    connection.Value.SetVisible(false);
                    connection.Value.GetAnimationGroupBuilder().Flip("back", 0f).MoveTo(m_ageCardDecks[connection.Key.Age], 0f);
                    connection.Value.Execute().GetAwaiter().GetResult();
                }
            });

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
            m_eventManager.Subscribe<OnCardDestroyed>(eventObj =>
            {
                if (m_pickCardLayer is not null)
                {
                    m_pickCardLayer.Visible = false;
                }
                MoveToDropCardDeck(eventObj.Card).GetAwaiter().GetResult();
            });
            m_eventManager.Subscribe<OnCardSold>(eventObj => {
                if (m_pickCardLayer is not null)
                {
                    m_pickCardLayer.Visible = false;
                }
                MoveToDropCardDeck(eventObj.Card).GetAwaiter().GetResult();
            });
            m_eventManager.Subscribe<CardNodeAvailableEvent>(eventObj => {
                var view = m_cards[eventObj.CardNode.CardObj];
                var group = view.GetAnimationGroupBuilder();
                group.Flip("front", 0.5f);
                view.Execute().GetAwaiter().GetResult();
            });
            m_eventManager.Subscribe<OnCardBuiltIntoWonder>(eventObj => {
                var cardView = m_cards[eventObj.Card];
                var connection = eventObj.WonderConnection;

                if (connection.CardTarget is null)
                {
                    throw new InvalidOperationException("Card target cannot be null when building into wonder");
                }

                var group = cardView.GetAnimationGroupBuilder();

                group.Unhighlight(false, 0.3f);
                cardView.Execute().GetAwaiter().GetResult();

                group.Flip("back", 0.2f);
                cardView.Execute().GetAwaiter().GetResult();

                group.MoveTo(connection.CardTarget, 0.5f);
                cardView.Execute().GetAwaiter().GetResult();
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
            view.SetVisible(true);
            var group = view.GetAnimationGroupBuilder();
            if (m_centerTargets.TryGetValue(nodeName, out var target))
            {
                group.MoveTo(target, 0.5f);
            }
            if (!hidden)
            {
                group.Flip("front", 0.5f);
            }
            await view.Execute();
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
        }

        private async Task MoveToDropCardDeck(Card card)
        {
            if (m_dropCardDeck is not null)
            {
                var view = m_cards[card];
                var animationBuilder = view.GetAnimationGroupBuilder().Unhighlight(false, 0.1f);
                await view.Execute();
                animationBuilder.MoveTo(m_dropCardDeck, 0.5f).Flip("back", 0.5f);
                await view.Execute();
                view.SetVisible(false);
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
                await m_player1Targets[card.GetType()].MoveCardToTarget(view);
            }
        }

        private async Task MoveToPlayer2(Card card)
        {
            if (m_player2Targets.ContainsKey(card.GetType()))
            {
                var view = m_cards[card];
                await m_player2Targets[card.GetType()].MoveCardToTarget(view);
            }
        }

        private readonly IDictionary<Card, IGameObjectView> m_cards;
        private readonly ICardConnector m_cardConnector;
        private readonly IGameEngineReceiver m_gameEngineReceiver;
        private readonly IEventManager m_eventManager;
        private readonly IPlayerCardHandlerFactory m_playerCardHandlerFactory;
        private readonly ISceneManager m_sceneManager;
        private readonly Dictionary<Type, IPlayerCardHandler> m_player1Targets;
        private readonly Dictionary<Type, IPlayerCardHandler> m_player2Targets;
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
