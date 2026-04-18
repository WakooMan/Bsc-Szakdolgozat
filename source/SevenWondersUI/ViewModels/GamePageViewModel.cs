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
using WebServer.Contract.Messages.Lobby.ClientMessages;
using WebServer.Contract.Messages.Lobby.ServerMessages;

namespace SevenWondersUI.ViewModels
{
    [QueryProperty(nameof(Player1), "Player1")]
    [QueryProperty(nameof(Player2), "Player2")]
    [QueryProperty(nameof(StartingPlayerId), "StartingPlayerId")]
    [QueryProperty(nameof(Seed), "Seed")]
    [QueryProperty(nameof(IsMultiplayer), "IsMultiplayer")]
    public class GamePageViewModel: BaseViewModel, IMessageHandler, IGameOverHandler
    {
        public GamePageViewModel(IGame game,
                                 IEngine engine,
                                 ISceneLoader sceneLoader,
                                 IAnimationManager animationManager,
                                 IPresenterStore presenterStore,
                                 IPlayerActionReceiverFactory playerActionReceiverFactory,
                                 IRandomGeneratorFactory randomGeneratorFactory,
                                 INavigationService navigationService,
                                 IClientHubService clientHubService)
        {
            Player1 = new PlayerInitModel();
            Player2 = new PlayerInitModel();
            m_playerActionReceiverFactory = playerActionReceiverFactory;
            m_presenterStore = presenterStore;
            m_clientHubService = clientHubService;
            m_navigationService = navigationService;
            m_animationManager = animationManager;
            m_sceneLoader = sceneLoader;
            m_game = game;
            m_engine = engine;
            m_randomGeneratorFactory = randomGeneratorFactory;
            m_lobbyResponseMessageHandlerDelegate = new LobbyResponseMessageHandlerDelegate<ExitGameResponseMessage>(HandleExitGameResponse);
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
            m_presenterStore.InitializePresenters(this);
            m_game.Initialize(m_randomGeneratorFactory.Create(IsMultiplayer ? RandomGeneratorType.Deterministic : RandomGeneratorType.Undeterministic, Seed), (Player1.Name, m_playerActionReceiverFactory.Create(Player1.PlayerType, Player1.Name)), (Player2.Name, m_playerActionReceiverFactory.Create(Player2.PlayerType, Player2.Name)), StartingPlayerId);
            m_presenterStore.SubscribePresentersToEvents();
            _ = Task.Run(m_game.GameLoop);
        }


        public void Register(IMessageRegisterer registerer)
        {
            registerer.Register(m_failureResponseMessageHandlerDelegate);
            registerer.Register(m_lobbyResponseMessageHandlerDelegate);
        }

        public void Unregister(IMessageRegisterer registerer)
        {
            registerer.Unregister(m_failureResponseMessageHandlerDelegate);
            registerer.Unregister(m_lobbyResponseMessageHandlerDelegate);
        }

        public async Task OnGameOver()
        {
            if (IsMultiplayer)
            {
                await m_clientHubService.InvokeLobbyCommand(new ExitGameRequestMessage());
            }
            else
            {
                await m_navigationService.NavigateToAsync("//MainPage");
            }
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

        private async Task<bool> HandleExitGameResponse(ExitGameResponseMessage message)
        {
            if (message.Success)
            {
                await MainThread.InvokeOnMainThreadAsync(async () =>
                {
                    await m_navigationService.NavigateToAsync("//LobbyMainPage", new Dictionary<string, object>() { { "Lobbies", message.Lobbies } });
                });
            }
            return message.Success;
        }

        private readonly ISceneLoader m_sceneLoader;
        private readonly IEngine m_engine;
        private readonly IGame m_game;
        private readonly IAnimationManager m_animationManager;
        private readonly IPresenterStore m_presenterStore;
        private readonly IPlayerActionReceiverFactory m_playerActionReceiverFactory;
        private readonly IRandomGeneratorFactory m_randomGeneratorFactory;
        private readonly INavigationService m_navigationService;
        private readonly IClientHubService m_clientHubService;
        private readonly LobbyResponseMessageHandlerDelegate<ExitGameResponseMessage> m_lobbyResponseMessageHandlerDelegate;
        private readonly LobbyResponseMessageHandlerDelegate<FailureResponseMessage> m_failureResponseMessageHandlerDelegate;
    }
}
