using CommunityToolkit.Maui.Views;
using SevenWonders.WebClient.Model;
using SevenWonders.WebClient.Model.Services;
using SevenWondersUI.Services;
using SevenWondersUI.Views;
using System.Collections.ObjectModel;
using System.Windows.Input;
using WebServer.Contract.DataTransferObjects;
using WebServer.Contract.Messages.Lobby.ClientMessages;
using WebServer.Contract.Messages.Lobby.ServerMessages;

namespace SevenWondersUI.ViewModels
{
    public class LobbyMainPageViewModel : BaseViewModel, IMessageHandler, IQueryAttributable
    {
        private bool _isFriendlyModeVisible = true;
        private bool _isCompetitiveModeVisible = false;
        private bool _isSearching = false;
        private string _searchTimerText = "00:00";
        private System.Timers.Timer? _timer;
        private int _secondsElapsed;

        public string FriendlyTabText => "Baráti mód";
        public string CompetitiveTabText => "Verseny mód";
        public string JoinButtonText => "Csatlakozás";
        public string CreateGameButtonText => "Játék létrehozása";
        public string SearchGameButtonText => "Játék keresése";
        public string StopSearchButtonText => "Játék keresés leállítása";
        public string LogoutButtonText => "Kijelentkezés";
        public string AvailableRoomsTitle => "Elérhető szobák";
        public string LeaderboardButtonText => "Ranglista";

        public bool IsFriendlyModeVisible
        {
            get => _isFriendlyModeVisible;
            set { _isFriendlyModeVisible = value; OnPropertyChanged(); }
        }

        public bool IsCompetitiveModeVisible
        {
            get => _isCompetitiveModeVisible;
            set { _isCompetitiveModeVisible = value; OnPropertyChanged(); }
        }

        public bool IsSearching
        {
            get => _isSearching;
            set { _isSearching = value; OnPropertyChanged(); OnPropertyChanged(nameof(IsSearchButtonVisible)); }
        }

        public bool IsSearchButtonVisible => !IsSearching;

        public string SearchTimerText
        {
            get => _searchTimerText;
            set { _searchTimerText = value; OnPropertyChanged(); }
        }

        private RoomModel? m_selectedRoom;

        public ObservableCollection<RoomModel> Rooms { get; set; }

        public RoomModel? SelectedRoom
        {
            get => m_selectedRoom;
            set
            {
                if (m_selectedRoom != value)
                {
                    if (m_selectedRoom is not null)
                    {
                        m_selectedRoom.BackgroundColor = Colors.White;
                        m_selectedRoom.TextColor = Colors.Black;
                    }
                    m_selectedRoom = value;
                    if (m_selectedRoom is not null)
                    {
                        m_selectedRoom.BackgroundColor = Colors.Black;
                        m_selectedRoom.TextColor = Colors.White;
                    }
                    OnPropertyChanged();
                    ((Command)JoinGameCommand).ChangeCanExecute();
                }
            }
        }

        public ICommand JoinGameCommand { get; }
        public ICommand CreateGameCommand { get; }

        public ICommand SelectFriendlyModeCommand { get; }
        public ICommand SelectCompetitiveModeCommand { get; }
        public ICommand StartSearchCommand { get; }
        public ICommand StopSearchCommand { get; }
        public ICommand LogoutCommand { get; }
        public ICommand ShowLeaderboardCommand { get; }

        public LobbyMainPageViewModel(IClientHubService clientHubService, 
                                      INavigationService navigationService, 
                                      IAuthService authService, 
                                      IPopupService popupService,
                                      CreateGamePopupWindow createGamePopupWindow)
        {
            m_clientHubService = clientHubService;
            m_navigationService = navigationService;
            m_authService = authService;
            m_popupService = popupService;
            m_createLobbyResponseMessageHandlerDelegate = new LobbyResponseMessageHandlerDelegate<CreateLobbyResponseMessage>(OnCreateLobbyResponseMessageReceived);
            m_joinLobbyResponseMessageHandlerDelegate = new LobbyResponseMessageHandlerDelegate<JoinLobbyResponseMessage>(OnJoinLobbyResponseMessageReceived);
            m_startMatchmakingResponseMessageHandlerDelegate = new LobbyResponseMessageHandlerDelegate<StartMatchmakingResponseMessage>(OnStartMatchmakingResponseMessageReceived);
            m_stopMatchmakingResponseMessageHandlerDelegate = new LobbyResponseMessageHandlerDelegate<StopMatchmakingResponseMessage>(OnStopMatchmakingResponseMessageReceived);
            m_lobbyUpdateMessageHandlerDelegate = new LobbyResponseMessageHandlerDelegate<LobbyUpdateMessage>(OnLobbyUpdateMessageReceived);
            m_failureResponseMessageHandlerDelegate = new LobbyResponseMessageHandlerDelegate<FailureResponseMessage>(OnFailureResponseMessageReceived);
            m_getLeaderboardResponseMessageHandlerDelegate = new LobbyResponseMessageHandlerDelegate<GetLeaderboardResponseMessage>(OnGetLeaderboardResponseMessageReceived);
            m_startGameResponseMessageHandler = new LobbyResponseMessageHandlerDelegate<StartGameResponseMessage>(OnStartGameResponseMessageReceived);
            Rooms = new ObservableCollection<RoomModel>();

            JoinGameCommand = new Command(JoinGame, () => SelectedRoom != null);

            CreateGameCommand = new Command(CreateGame);

            SelectFriendlyModeCommand = new Command(SelectFriendlyMode);

            SelectCompetitiveModeCommand = new Command(SelectCompetitiveMode);

            StartSearchCommand = new Command(StartSearch);
            StopSearchCommand = new Command(StopSearch);
            LogoutCommand = new Command(Logout);
            ShowLeaderboardCommand = new Command(ShowLeaderboard);
        }

        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            LobbyDto[]? lobbies = query["Lobbies"] as LobbyDto[];
            if (lobbies is not null)
            {
                SetRooms(lobbies);
            }
        }

        private async void Logout()
        {
            await m_clientHubService.Disconnect();
            await m_authService.LogoutAsync();
            await m_navigationService.NavigateToAsync("//LoginPage");
        }

        public void Register(IMessageRegisterer registerer)
        {
            registerer.Register(m_createLobbyResponseMessageHandlerDelegate);
            registerer.Register(m_joinLobbyResponseMessageHandlerDelegate);
            registerer.Register(m_startMatchmakingResponseMessageHandlerDelegate);
            registerer.Register(m_stopMatchmakingResponseMessageHandlerDelegate);
            registerer.Register(m_lobbyUpdateMessageHandlerDelegate);
            registerer.Register(m_failureResponseMessageHandlerDelegate);
            registerer.Register(m_getLeaderboardResponseMessageHandlerDelegate);
            registerer.Register(m_startGameResponseMessageHandler);
        }

        public void Unregister(IMessageRegisterer registerer)
        {
            registerer.Unregister(m_createLobbyResponseMessageHandlerDelegate);
            registerer.Unregister(m_joinLobbyResponseMessageHandlerDelegate);
            registerer.Unregister(m_startMatchmakingResponseMessageHandlerDelegate);
            registerer.Unregister(m_stopMatchmakingResponseMessageHandlerDelegate);
            registerer.Unregister(m_lobbyUpdateMessageHandlerDelegate);
            registerer.Unregister(m_failureResponseMessageHandlerDelegate);
            registerer.Unregister(m_getLeaderboardResponseMessageHandlerDelegate);
            registerer.Unregister(m_startGameResponseMessageHandler);
        }

        private async void CreateGame()
        {
            CreateGamePopupWindow createGamePopupWindow = new CreateGamePopupWindow(new CreateGamePopupViewModel());
            await m_popupService.ShowAsync(createGamePopupWindow);
            if (createGamePopupWindow.ViewModel.CreateActivated)
            {
                await m_clientHubService.InvokeLobbyCommand(new CreateLobbyRequestMessage(createGamePopupWindow.ViewModel.RoomName));
            }
        }

        private void SelectFriendlyMode()
        {
            IsFriendlyModeVisible = true;
            IsCompetitiveModeVisible = false;
        }

        private void SelectCompetitiveMode()
        {
            IsFriendlyModeVisible = false;
            IsCompetitiveModeVisible = true;
        }

        private async void JoinGame()
        {
            if (SelectedRoom != null)
            {
                await m_clientHubService.InvokeLobbyCommand(new JoinLobbyRequestMessage(SelectedRoom.Code));
            }
        }

        private async Task<bool> OnStartMatchmakingResponseMessageReceived(StartMatchmakingResponseMessage message)
        {
            if (message.Success)
            {
                await MainThread.InvokeOnMainThreadAsync(() =>
                {
                    IsSearching = true;
                    _secondsElapsed = 0;
                    SearchTimerText = "00:00";

                    _timer = new System.Timers.Timer(1000);
                    _timer.Elapsed += (s, e) =>
                    {
                        _secondsElapsed++;
                        var time = TimeSpan.FromSeconds(_secondsElapsed);
                        SearchTimerText = time.ToString(@"mm\:ss");
                    };
                    _timer.Start();
                });
            }

            return message.Success;
        }

        private async Task<bool> OnStopMatchmakingResponseMessageReceived(StopMatchmakingResponseMessage message)
        {
            if (message.Success)
            {
                await MainThread.InvokeOnMainThreadAsync(() =>
                {
                    _timer?.Stop();
                    _timer?.Dispose();
                    IsSearching = false;
                });
            }

            return message.Success;
        }

        private async Task<bool> OnJoinLobbyResponseMessageReceived(JoinLobbyResponseMessage message)
        {
            if (message.Success)
            {
                await MainThread.InvokeOnMainThreadAsync(async () =>
                {
                    await m_navigationService.NavigateToAsync("//LobbyPage", new Dictionary<string, object>
                    {
                        { "LobbyDto", message.LobbyDto }
                    });
                });
            }
            return message.Success;
        }

        private async Task<bool> OnCreateLobbyResponseMessageReceived(CreateLobbyResponseMessage message)
        {
            if (message.Success)
            {
                await MainThread.InvokeOnMainThreadAsync(async () =>
                {
                    await m_navigationService.NavigateToAsync("//LobbyPage", new Dictionary<string, object>
                    {
                        { "LobbyDto", message.LobbyDto }
                    });
                });
            }
            return message.Success;
        }

        private async Task<bool> OnLobbyUpdateMessageReceived(LobbyUpdateMessage message)
        {
            if (message.Success)
            {
                await MainThread.InvokeOnMainThreadAsync(() =>
                {
                    SetRooms(message.Lobbies);
                });
            }

            return message.Success;
        }

        private async Task<bool> OnStartGameResponseMessageReceived(StartGameResponseMessage message)
        {
            if (message.Success)
            {
                await MainThread.InvokeOnMainThreadAsync(async () =>
                {
                    await m_navigationService.NavigateToAsync("//MultiplayerGamePage", new Dictionary<string, object>
                    {
                        { "Player1Name", message.Player1Name },
                        { "Player2Name", message.Player2Name },
                        { "Player1Type", message.Player1Type },
                        { "Player2Type", message.Player2Type },
                        { "StartingPlayerId", message.StartingPlayerId },
                        { "Seed", message.Seed }
                    });
                });
            }
            return message.Success;
        }

        private async void StartSearch()
        {
            await m_clientHubService.InvokeLobbyCommand(new StartMatchmakingRequestMessage());
        }

        private async void StopSearch()
        {
            await m_clientHubService.InvokeLobbyCommand(new StopMatchmakingRequestMessage());
        }

        private void SetRooms(LobbyDto[] lobbies)
        {
            Rooms.Clear();
            foreach (LobbyDto lobbyDto in lobbies)
            {
                Rooms.Add(new RoomModel
                {
                    RoomName = lobbyDto.Name,
                    Code = lobbyDto.Code,
                    HostName = lobbyDto.Members.FirstOrDefault(member => member.IsHost)?.UserName ?? "Unknown",
                    BackgroundColor = Colors.White,
                    TextColor = Colors.Black
                });
            }
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

        private async void ShowLeaderboard()
        {
            await m_clientHubService.InvokeLobbyCommand(new GetLeaderboardRequestMessage());
        }

        private async Task<bool> OnGetLeaderboardResponseMessageReceived(GetLeaderboardResponseMessage message)
        {
            if (message.Success)
            {
                await MainThread.InvokeOnMainThreadAsync(async () =>
                {
                    var popup = new LeaderboardPopupWindow(new LeaderboardPopupViewModel(message.Entries));
                    await m_popupService.ShowAsync(popup);
                });
            }
            return message.Success;
        }

        private readonly LobbyResponseMessageHandlerDelegate<StartGameResponseMessage> m_startGameResponseMessageHandler;
        private readonly LobbyResponseMessageHandlerDelegate<CreateLobbyResponseMessage> m_createLobbyResponseMessageHandlerDelegate;
        private readonly LobbyResponseMessageHandlerDelegate<JoinLobbyResponseMessage> m_joinLobbyResponseMessageHandlerDelegate;
        private readonly LobbyResponseMessageHandlerDelegate<StartMatchmakingResponseMessage> m_startMatchmakingResponseMessageHandlerDelegate;
        private readonly LobbyResponseMessageHandlerDelegate<StopMatchmakingResponseMessage> m_stopMatchmakingResponseMessageHandlerDelegate;
        private readonly LobbyResponseMessageHandlerDelegate<LobbyUpdateMessage> m_lobbyUpdateMessageHandlerDelegate;
        private readonly LobbyResponseMessageHandlerDelegate<FailureResponseMessage> m_failureResponseMessageHandlerDelegate;
        private readonly LobbyResponseMessageHandlerDelegate<GetLeaderboardResponseMessage> m_getLeaderboardResponseMessageHandlerDelegate;
        private readonly IClientHubService m_clientHubService;
        private readonly INavigationService m_navigationService;
        private readonly IAuthService m_authService;
        private readonly IPopupService m_popupService;
    }
}