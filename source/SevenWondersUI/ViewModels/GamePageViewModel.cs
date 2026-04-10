using GameLogic;
using SevenWonders.GameEngine;
using SevenWonders.GameEngine.Components;
using SevenWonders.Presenter;
using SevenWonders.Presenter.Presenters;
using SevenWondersUI.Services;

namespace SevenWondersUI.ViewModels
{
    [QueryProperty(nameof(Player1Name), "Player1Name")]
    [QueryProperty(nameof(Player2Name), "Player2Name")]
    public class GamePageViewModel: BaseViewModel
    {
        public GamePageViewModel(IGame game, IEngine engine, ISceneLoader sceneLoader, IAnimationManager animationManager, IPresenter presenter)
        {
            Player1Name = string.Empty;
            Player2Name = string.Empty;
            m_presenter = presenter;
            m_animationManager = animationManager;
            m_sceneLoader = sceneLoader;
            m_game = game;
            m_engine = engine;
        }

        public string Player1Name { get; set; }
        public string Player2Name { get; set; }

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
            m_game.Initialize(Player1Name, Player2Name);
            m_presenter.SubscribeToEvents();
            _ = Task.Run(m_game.GameLoop);
        }

        private readonly ISceneLoader m_sceneLoader;
        private readonly IEngine m_engine;
        private readonly IGame m_game;
        private readonly IAnimationManager m_animationManager;
        private readonly IPresenter m_presenter;
    }
}
