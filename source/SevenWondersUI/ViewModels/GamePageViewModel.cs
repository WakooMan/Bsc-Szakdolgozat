using CommunityToolkit.Maui.Views;
using GameLogic;
using SevenWonders.Common;
using SevenWonders.GameEngine;
using SevenWonders.GameEngine.Components;
using SevenWonders.Presenter;
using SevenWonders.Presenter.PlayerActionReceivers;
using SevenWonders.Presenter.Presenters;
using SevenWonders.Presenter.Presenters.Factories;
using SevenWonders.WebClient.Model;
using SevenWonders.WebClient.Model.Services;
using SevenWondersUI.Services;
using SevenWondersUI.Views;
using WebServer.Contract.Messages.Lobby.ServerMessages;

namespace SevenWondersUI.ViewModels
{
    [QueryProperty(nameof(Player1), "Player1")]
    [QueryProperty(nameof(Player2), "Player2")]
    [QueryProperty(nameof(StartingPlayerId), "StartingPlayerId")]
    [QueryProperty(nameof(Seed), "Seed")]
    [QueryProperty(nameof(IsMultiplayer), "IsMultiplayer")]
    public class GamePageViewModel: BaseViewModel, IMessageHandler
    {
        public GamePageViewModel(IGame game,
                                 IEngine engine,
                                 ISceneLoader sceneLoader,
                                 IAnimationManager animationManager,
                                 IPresenter presenter,
                                 IPlayerActionReceiverFactory playerActionReceiverFactory,
                                 IRandomGeneratorFactory randomGeneratorFactory,
                                 IPresenterFactory presenterFactory)
        {
            Player1 = new PlayerInitModel();
            Player2 = new PlayerInitModel();
            m_playerActionReceiverFactory = playerActionReceiverFactory;
            m_presenter = presenter;
            m_animationManager = animationManager;
            m_sceneLoader = sceneLoader;
            m_game = game;
            m_engine = engine;
            m_randomGeneratorFactory = randomGeneratorFactory;
            m_presenterFactory = presenterFactory;
            m_failureResponseMessageHandlerDelegate = new LobbyResponseMessageHandlerDelegate<FailureResponseMessage>(OnFailureResponseMessageReceived);

        }

        public int StartingPlayerId { get; set; }

        public PlayerInitModel Player1 { get; set; }
        public PlayerInitModel Player2 { get; set; }
        public bool IsMultiplayer { get; set; }
        public int Seed { get; set; }

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
            m_presenterFactory.Initialize(IsMultiplayer);
            m_presenter.Initialize();
            m_game.Initialize(m_randomGeneratorFactory.Create(IsMultiplayer ? RandomGeneratorType.Deterministic : RandomGeneratorType.Undeterministic, Seed), (Player1.Name, m_playerActionReceiverFactory.Create(Player1.PlayerType, Player1.Name)), (Player2.Name, m_playerActionReceiverFactory.Create(Player2.PlayerType, Player2.Name)), StartingPlayerId);
            m_presenter.SubscribeToEvents();
            _ = Task.Run(m_game.GameLoop);
        }


        public void Register(IMessageRegisterer registerer)
        {
            registerer.Register(m_failureResponseMessageHandlerDelegate);
        }

        public void Unregister(IMessageRegisterer registerer)
        {
            registerer.Unregister(m_failureResponseMessageHandlerDelegate);
        }

        private async Task<bool> OnFailureResponseMessageReceived(FailureResponseMessage message)
        {
            await MainThread.InvokeOnMainThreadAsync(async () =>
            {
                var popup = new ErrorPopupWindow(new ErrorPopupViewModel(message.Message));
                var page = Application.Current?.MainPage;
                if (page is not null)
                {
                    await page.ShowPopupAsync(popup);
                }
            });
            return false;
        }

        private readonly ISceneLoader m_sceneLoader;
        private readonly IEngine m_engine;
        private readonly IGame m_game;
        private readonly IAnimationManager m_animationManager;
        private readonly IPresenter m_presenter;
        private readonly IPlayerActionReceiverFactory m_playerActionReceiverFactory;
        private readonly IRandomGeneratorFactory m_randomGeneratorFactory;
        private readonly IPresenterFactory m_presenterFactory;
        private readonly LobbyResponseMessageHandlerDelegate<FailureResponseMessage> m_failureResponseMessageHandlerDelegate;
    }
}
