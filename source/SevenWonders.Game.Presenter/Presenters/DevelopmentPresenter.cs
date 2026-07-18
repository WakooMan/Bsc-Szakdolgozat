using SevenWonders.Game.Logic.Elements;
using SevenWonders.Game.Logic.Elements.Modifiers;
using SevenWonders.Game.Logic.Events;
using SevenWonders.Game.Logic.Events.GameEvents;
using SevenWonders.Game.Presenter.Connectors;
using SevenWonders.Game.Presenter.Connectors.Developments;
using SevenWonders.Game.Presenter.Presenters.Factories;
using SevenWonders.Game.Presenter.Presenters.Handlers;
using SevenWonders.Game.Presenter.Views;
using SevenWonders.Game.Engine.SceneObjects;

namespace SevenWonders.Game.Presenter.Presenters
{
    public class DevelopmentPresenter : IPresenter
    {
        public DevelopmentPresenter(IDevelopmentConnector developmentConnector, IGameEngineReceiver gameEngineReceiver, IEventManager eventManager, IDevelopmentHandlerFactory developmentHandlerFactory)
        {
            m_developmentConnector = developmentConnector;
            m_gameEngineReceiver = gameEngineReceiver;
            m_eventManager = eventManager;
            m_developments = new Dictionary<Development, IGameObjectView>();
            m_militaryBoardTargets = new List<GameObject>();
            m_developmentHandlerFactory = developmentHandlerFactory;
        }

        public void Initialize()
        {
            foreach (var connection in m_developmentConnector.ReceiveDevelopmentConnection())
            {
                m_developments.Add(connection);
            }

            m_militaryBoardTargets.AddRange(m_gameEngineReceiver.ReceiveGameObjects("dev", 5));

            m_player1DevelopmentHandler = m_developmentHandlerFactory.Create(m_gameEngineReceiver.ReceiveGraphicsLayer("background"), m_gameEngineReceiver.ReceiveGameObject("player1Development"));
            m_player2DevelopmentHandler = m_developmentHandlerFactory.Create(m_gameEngineReceiver.ReceiveGraphicsLayer("background"), m_gameEngineReceiver.ReceiveGameObject("player2Development"));
            m_developmentDeck = m_gameEngineReceiver.ReceiveGameObject("developmentDeck");

        }

        public void SubscribeToEvents()
        {
            m_eventManager.Subscribe<OnGameInitialized>(eventObj =>
            {
                if (eventObj.GameContext.DevelopmentList is not null && m_developmentDeck is not null)
                {
                    eventObj.GameContext.DevelopmentList.Developments.ForEach(development =>
                    {
                        IGameObjectView gameObjectView = m_developments[development];
                        gameObjectView.GetAnimationGroupBuilder().MoveTo(m_developmentDeck, 0f).Flip("back", 0f);
                        gameObjectView.SetVisible(false);
                        gameObjectView.Execute();
                    });
                }

                var developments = eventObj.GameContext.MilitaryBoard.Developments;
                if (eventObj.GameContext.MilitaryBoard is not null && m_militaryBoardTargets.Count == developments.Count)
                {
                    for (int i = 0; i < developments.Count; i++)
                    {
                        IGameObjectView gameObjectView = m_developments[developments[i]];
                        gameObjectView.GetAnimationGroupBuilder().MoveTo(m_militaryBoardTargets[i], 0f);
                        gameObjectView.Execute();
                    }
                }
            });

            m_eventManager.Subscribe<OnPlayerDevelopmentReceived>(eventObj =>
            {
                MoveToPlayer(eventObj.Player, eventObj.Development).GetAwaiter().GetResult();
            });
        }

        private async Task MoveToPlayer(Player player, Development development)
        {
            if (player.Id == 1 && m_player1DevelopmentHandler is not null)
            {
                await m_player1DevelopmentHandler.MoveDevelopmentToTarget(m_developments[development]);
            }
            if (player.Id == 2 && m_player2DevelopmentHandler is not null)
            {
                await m_player2DevelopmentHandler.MoveDevelopmentToTarget(m_developments[development]);
            }
        }

        private readonly IEventManager m_eventManager;
        private readonly IGameEngineReceiver m_gameEngineReceiver;
        private readonly IDevelopmentConnector m_developmentConnector;
        private readonly IDevelopmentHandlerFactory m_developmentHandlerFactory;
        private readonly IDictionary<Development, IGameObjectView> m_developments;
        private readonly List<GameObject> m_militaryBoardTargets;
        private IDevelopmentHandler? m_player1DevelopmentHandler;
        private IDevelopmentHandler? m_player2DevelopmentHandler;
        private GameObject? m_developmentDeck;
    }
}
