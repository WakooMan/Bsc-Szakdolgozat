using GameLogic.Elements;
using GameLogic.Elements.Modifiers;
using GameLogic.Events;
using GameLogic.Events.GameEvents;
using SevenWonders.GameEngine;
using SevenWonders.Presenter.Connectors;
using SevenWonders.Presenter.Connectors.Developments;
using SevenWonders.Presenter.Views;

namespace SevenWonders.Presenter.Presenters
{
    public class DevelopmentPresenter : IPresenter
    {
        public DevelopmentPresenter(IDevelopmentConnector developmentConnector, IGameEngineReceiver gameEngineReceiver, IEventManager eventManager)
        {
            m_developmentConnector = developmentConnector;
            m_gameEngineReceiver = gameEngineReceiver;
            m_eventManager = eventManager;
            m_developments = new Dictionary<Development, IGameObjectView>();
            m_militaryBoardTargets = new List<GameObject>();
        }

        public void Initialize()
        {
            foreach (var connection in m_developmentConnector.ReceiveDevelopmentConnection())
            {
                m_developments.Add(connection);
            }

            m_militaryBoardTargets.AddRange(m_gameEngineReceiver.ReceiveGameObjects("dev", 3));

            m_player1Target = m_gameEngineReceiver.ReceiveGameObject("player1Development");
            m_player2Target = m_gameEngineReceiver.ReceiveGameObject("player2Development");
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
                        gameObjectView.GetAnimationGroupBuilder().MoveTo(m_developmentDeck, 0f).Flip(1, 0f);
                        gameObjectView.Execute();
                    });
                }

                if (eventObj.GameContext.MilitaryBoard is not null && m_militaryBoardTargets.Count == 3)
                {
                    var developments = eventObj.GameContext.MilitaryBoard.Developments;
                    if (developments.Count == 3)
                    {
                        for (int i = 0; i < 3; i++)
                        {
                            IGameObjectView gameObjectView = m_developments[developments[i]];
                            gameObjectView.GetAnimationGroupBuilder().MoveTo(m_militaryBoardTargets[i], 0f);
                            gameObjectView.Execute();
                        }
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
            if (player.Id == 1)
            {
                await MoveToPlayer(m_player1Target, development);
            }
            if (player.Id == 2)
            {
                await MoveToPlayer(m_player2Target, development);
            }
        }

        private async Task MoveToPlayer(GameObject target, Development development)
        {
            IGameObjectView gameObjectView = m_developments[development];
            gameObjectView.GetAnimationGroupBuilder().MoveTo(target, 0.5f);
            await gameObjectView.Execute();
        }

        private readonly IEventManager m_eventManager;
        private readonly IGameEngineReceiver m_gameEngineReceiver;
        private readonly IDevelopmentConnector m_developmentConnector;
        private readonly IDictionary<Development, IGameObjectView> m_developments;
        private readonly List<GameObject> m_militaryBoardTargets;
        private GameObject? m_player1Target;
        private GameObject? m_player2Target;
        private GameObject? m_developmentDeck;
    }
}
