using GameLogic;
using GameLogic.Interfaces;
using SevenWonders.AI.Model.Cache;
using SevenWonders.Common;
using SevenWonders.GameEngine;
using SevenWonders.GameEngine.Components;
using SevenWonders.Presenter.PlayerActionReceivers;
using SevenWonders.Presenter.Presenters;
using SkiaSharp;
using SkiaSharp.Views.Maui;
using System.Numerics;

namespace SevenWonders.Presenter
{
    public class GameHandler : IGameHandler
    {
        public GameHandler(
            ISceneLoader sceneLoader,
            IEngine engine,
            IGame game,
            IAnimationManager animationManager,
            IPresenterStore presenterStore,
            IPlayerActionReceiverFactory playerActionReceiverFactory,
            IRandomGeneratorFactory randomGeneratorFactory,
            IAIDecisionHandlerCache aIDecisionHandlerCache)
        {
            m_sceneLoader = sceneLoader;
            m_engine = engine;
            m_game = game;
            m_animationManager = animationManager;
            m_presenterStore = presenterStore;
            m_playerActionReceiverFactory = playerActionReceiverFactory;
            m_randomGeneratorFactory = randomGeneratorFactory;
            m_aiDecisionHandlerCache = aIDecisionHandlerCache;
            m_cancellationTokenSource = null;
        }

        public void InitializeEngine()
        {
            m_engine.RegisterSubSystem(m_animationManager);
        }

        public async Task StartGame(string player1Name, PlayerType player1Type, string player2Name, PlayerType player2Type, RandomGeneratorType randomGeneratorType, int seed, int startingPlayerId, IGameOverHandler gameOverHandler)
        {
            m_cancellationTokenSource?.Cancel();
            m_cancellationTokenSource?.Dispose();
            m_cancellationTokenSource = null;

            foreach (Scene scene in await m_sceneLoader.LoadScenes())
            {
                m_engine.SceneManager.RegisterScene(scene);
            }

            Scene? firstScene = m_engine.SceneManager.Scenes.FirstOrDefault();

            if (firstScene is null)
            {
                throw new InvalidOperationException("No scenes were loaded.");
            }

            m_engine.SceneManager.SetCurrentScene(firstScene);
            m_engine.Startup();

            IRandomGenerator randomGenerator = m_randomGeneratorFactory.Create(randomGeneratorType, seed);
            IPlayerActionReceiver player1ActionReceiver = m_playerActionReceiverFactory.Create(player1Type, player1Name);
            IPlayerActionReceiver player2ActionReceiver = m_playerActionReceiverFactory.Create(player2Type, player2Name);
            m_game.Initialize(randomGenerator, (player1Name, player1ActionReceiver), (player2Name, player2ActionReceiver), startingPlayerId);
            m_presenterStore.InitializePresenters(gameOverHandler);
            m_presenterStore.SubscribePresentersToEvents();

            InitializeAI(player2Type);

            m_cancellationTokenSource = new CancellationTokenSource();
            _ = Task.Run(m_game.GameLoop, m_cancellationTokenSource.Token);
        }

        public void StopGame()
        {
            m_cancellationTokenSource?.Cancel();
            m_cancellationTokenSource?.Dispose();
            m_cancellationTokenSource = null;
            Scene? current = m_engine.SceneManager.CurrentScene;
            if (current is not null)
            {
                m_engine.SceneManager.FreeASceneByID(current.Id);
            }
            m_engine.Shutdown();
        }

        public void Render(SKCanvas sKCanvas)
        {
            m_engine.SceneManager.Render(sKCanvas);
        }

        public void OnTouchEvent(SKTouchEventArgs eventArgs)
        {
            m_engine.InputManager.OnTouchEvent(eventArgs);
        }

        public void Resize(Vector2 newSize)
        {
            if (m_engine.SceneManager.CurrentScene is not null)
            {
                m_engine.SceneManager.CurrentScene.Resize(newSize);
            }
        }

        public void SubscribeRedrawRequested(EventHandler handler)
        {
            m_engine.RedrawRequested += handler;
        }

        public void UnsubscribeRedrawRequested(EventHandler handler)
        {
            m_engine.RedrawRequested -= handler;
        }

        private void InitializeAI(PlayerType playerType)
        {
            switch (playerType)
            {
                case PlayerType.EasyAI:
                    m_aiDecisionHandlerCache.EasyAI.Initialize(2);
                    break;
                case PlayerType.MediumAI:
                    m_aiDecisionHandlerCache.MediumAI.Initialize(2);
                    break;
                case PlayerType.HardAI:
                    m_aiDecisionHandlerCache.HardAI.Initialize(2);
                    break;
            }
        }

        private readonly ISceneLoader m_sceneLoader;
        private readonly IEngine m_engine;
        private readonly IGame m_game;
        private readonly IAnimationManager m_animationManager;
        private readonly IPresenterStore m_presenterStore;
        private readonly IPlayerActionReceiverFactory m_playerActionReceiverFactory;
        private readonly IRandomGeneratorFactory m_randomGeneratorFactory;
        private readonly IAIDecisionHandlerCache m_aiDecisionHandlerCache;
        private CancellationTokenSource? m_cancellationTokenSource;
    }
}
