using GameLogic;
using GameLogic.Interfaces;
using SevenWonders.Common;
using SevenWonders.GameEngine;
using SevenWonders.GameEngine.Components;
using SevenWonders.Presenter;
using SevenWonders.Presenter.PlayerActionReceivers;
using SevenWonders.Presenter.Presenters;

namespace SevenWondersUI.ViewModels
{
    public abstract class BaseGamePageViewModel: BaseViewModel, IGameOverHandler, IQueryAttributable
    {
        protected BaseGamePageViewModel(IGame game,
                                        IEngine engine,
                                        ISceneLoader sceneLoader,
                                        IAnimationManager animationManager,
                                        IPresenterStore presenterStore,
                                        IPlayerActionReceiverFactory playerActionReceiverFactory,
                                        IRandomGeneratorFactory randomGeneratorFactory)
        {
            Player1Name = string.Empty;
            Player2Name = string.Empty;
            m_playerActionReceiverFactory = playerActionReceiverFactory;
            m_presenterStore = presenterStore;
            m_animationManager = animationManager;
            m_sceneLoader = sceneLoader;
            m_game = game;
            m_engine = engine;
            m_randomGeneratorFactory = randomGeneratorFactory;
        }

        public string Player1Name { get; set; }
        public string Player2Name { get; set; }

        public IEngine Engine => m_engine;

        protected abstract RandomGeneratorType RandomGeneratorType { get; }
        protected abstract PlayerType Player1Type { get; }
        protected abstract PlayerType Player2Type { get; }

        protected virtual int Seed { get { return 0; } }

        protected virtual int StartingPlayerId { get { return 1; } }

        public async Task Initialize()
        {
            // Wait for query attributes to be applied before initializing the game and presenters

            m_engine.RegisterSubSystem(m_animationManager);

            foreach (Scene scene in await m_sceneLoader.LoadScenes())
            {
                m_engine.SceneManager.RegisterScene(scene);
            }

            Scene? firstScene = m_engine.SceneManager.Scenes.FirstOrDefault();

            if (firstScene != null)
            {
                m_engine.SceneManager.SetCurrentScene(firstScene);
            }

            InitializeGame();
            _ = Task.Run(m_game.GameLoop);
        }

        public abstract Task OnGameOver();
        public virtual void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            if (query.TryGetValue("Player1Name", out object? player1NameObj) && player1NameObj is string player1Name)
            {
                Player1Name = player1Name;
            }
            if (query.TryGetValue("Player2Name", out object? player2NameObj) && player2NameObj is string player2Name)
            {
                Player2Name = player2Name;
            }
        }

        protected virtual void InitializeGame()
        {
            m_engine.Startup();
            IRandomGenerator randomGenerator = m_randomGeneratorFactory.Create(RandomGeneratorType, Seed);
            IPlayerActionReceiver player1ActionReceiver = m_playerActionReceiverFactory.Create(Player1Type, Player1Name);
            IPlayerActionReceiver player2ActionReceiver = m_playerActionReceiverFactory.Create(Player2Type, Player2Name);
            m_game.Initialize(randomGenerator, (Player1Name, player1ActionReceiver), (Player2Name, player2ActionReceiver), StartingPlayerId);
            m_presenterStore.InitializePresenters(this);
            m_presenterStore.SubscribePresentersToEvents();
        }

        private readonly ISceneLoader m_sceneLoader;
        private readonly IEngine m_engine;
        private readonly IGame m_game;
        private readonly IAnimationManager m_animationManager;
        private readonly IPresenterStore m_presenterStore;
        private readonly IPlayerActionReceiverFactory m_playerActionReceiverFactory;
        private readonly IRandomGeneratorFactory m_randomGeneratorFactory;
    }
}
