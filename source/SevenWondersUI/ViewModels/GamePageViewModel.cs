using GameLogic;
using SevenWonders.Common;
using SevenWonders.GameEngine;
using SevenWonders.GameEngine.Components;
using SevenWonders.Presenter.PlayerActionReceivers;
using SevenWonders.Presenter.Presenters;

namespace SevenWondersUI.ViewModels
{
    [QueryProperty(nameof(Player1), "Player1")]
    [QueryProperty(nameof(Player2), "Player2")]
    public class GamePageViewModel: BaseViewModel
    {
        public GamePageViewModel(IGame game, IEngine engine, ISceneLoader sceneLoader, IAnimationManager animationManager, IPresenter presenter, IPlayerActionReceiverFactory playerActionReceiverFactory)
        {
            Player1 = new PlayerInitModel();
            Player2 = new PlayerInitModel();
            m_playerActionReceiverFactory = playerActionReceiverFactory;
            m_presenter = presenter;
            m_animationManager = animationManager;
            m_sceneLoader = sceneLoader;
            m_game = game;
            m_engine = engine;
        }

        public PlayerInitModel Player1 { get; set; }
        public PlayerInitModel Player2 { get; set; }

        public IEngine Engine => m_engine;

        public async Task Initialize()
        {
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

            m_engine.Startup();
            m_presenter.Initialize();
            m_game.Initialize((Player1.Name, m_playerActionReceiverFactory.Create(Player1.PlayerType)), (Player2.Name, m_playerActionReceiverFactory.Create(Player2.PlayerType)));
            m_presenter.SubscribeToEvents();
            _ = Task.Run(m_game.GameLoop);
        }

        private readonly ISceneLoader m_sceneLoader;
        private readonly IEngine m_engine;
        private readonly IGame m_game;
        private readonly IAnimationManager m_animationManager;
        private readonly IPresenter m_presenter;
        private readonly IPlayerActionReceiverFactory m_playerActionReceiverFactory;
    }
}
