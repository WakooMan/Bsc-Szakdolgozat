using SevenWonders.Common;
using SevenWonders.Game.Presenter;
using SevenWonders.Web.Client.Model;
using SevenWonders.Web.Client.Model.Services;
using SevenWonders.UI.Services;
using SevenWonders.UI.Views;
using SevenWonders.Web.Server.Contract.Messages.Lobby.ClientMessages;
using SevenWonders.Web.Server.Contract.Messages.Lobby.ServerMessages;

namespace SevenWonders.UI.ViewModels
{
    public class MultiplayerGamePageViewModel : BaseGamePageViewModel, IMessageHandler
    {
        protected override int Seed { get { return m_seed; } }
        protected override int StartingPlayerId { get { return m_startingPlayerId; } }
        protected override RandomGeneratorType RandomGeneratorType => RandomGeneratorType.Deterministic;

        protected override PlayerType Player1Type { get { return m_player1Type; } }

        protected override PlayerType Player2Type { get { return m_player2Type; } }

        public MultiplayerGamePageViewModel(IGameHandler gameHandler,
                                            INavigationService navigationService,
                                            IClientHubService clientHubService,
                                            IPopupService popupService) : base(gameHandler, navigationService)
        {
            m_seed = -1;
            m_startingPlayerId = -1;
            m_player1Type =  PlayerType.Unknown;
            m_player2Type = PlayerType.Unknown;
            m_navigationService = navigationService;
            m_clientHubService = clientHubService;
            m_lobbyResponseMessageHandlerDelegate = new LobbyResponseMessageHandlerDelegate<ExitGameResponseMessage>(HandleExitGameResponse);
            m_failureResponseMessageHandlerDelegate = new LobbyResponseMessageHandlerDelegate<FailureResponseMessage>(OnFailureResponseMessageReceived);
            m_popupService=popupService;
        }

        public override async Task OnGameOver()
        {
            await m_clientHubService.InvokeLobbyCommand(new ExitGameRequestMessage());
        }

        public override void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            if (query.TryGetValue("Player1Type", out object? player1TypeObj) && player1TypeObj is PlayerType player1Type)
            {
                m_player1Type = player1Type;
            }
            if (query.TryGetValue("Player2Type", out object? player2TypeObj) && player2TypeObj is PlayerType player2Type)
            {
                m_player2Type = player2Type;
            }
            if (query.TryGetValue("Seed", out object? seedObj) && seedObj is int seed)
            {
                m_seed = seed;
            }
            if (query.TryGetValue("StartingPlayerId", out object? startingPlayerIdObj) && startingPlayerIdObj is int startingPlayerId)
            {
                m_startingPlayerId = startingPlayerId;
            }
            base.ApplyQueryAttributes(query);
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

        private async Task<bool> OnFailureResponseMessageReceived(FailureResponseMessage message)
        {
            await MainThread.InvokeOnMainThreadAsync(async () =>
            {
                var popup = new ErrorPopupWindow(new ErrorPopupViewModel(message.Message));
                await m_popupService.ShowAsync(popup);
            });
            return false;
        }

        private async Task<bool> HandleExitGameResponse(ExitGameResponseMessage message)
        {
            if (message.Success)
            {
                await MainThread.InvokeOnMainThreadAsync(async () =>
                {
                    GameHandler.StopGame();
                    await m_navigationService.NavigateToAsync("//LobbyMainPage", new Dictionary<string, object>() { { "Lobbies", message.Lobbies } });
                });
            }
            return message.Success;
        }

        private int m_seed;
        private int m_startingPlayerId;
        private PlayerType m_player1Type;
        private PlayerType m_player2Type;
        private readonly LobbyResponseMessageHandlerDelegate<ExitGameResponseMessage> m_lobbyResponseMessageHandlerDelegate;
        private readonly LobbyResponseMessageHandlerDelegate<FailureResponseMessage> m_failureResponseMessageHandlerDelegate;
        private readonly INavigationService m_navigationService;
        private readonly IClientHubService m_clientHubService;
        private readonly IPopupService m_popupService;
    }
}
