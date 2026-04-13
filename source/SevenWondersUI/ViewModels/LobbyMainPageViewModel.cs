using SevenWonders.WebClient.Model;
using SevenWondersUI.Services;
using System.Collections.ObjectModel;
using System.Windows.Input;
using WebServer.Contract.Messages.Lobby.ClientMessages;
using WebServer.Contract.Messages.Lobby.ServerMessages;

namespace SevenWondersUI.ViewModels
{
    public class LobbyMainPageViewModel : BaseViewModel, IMessageHandler
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
                        m_selectedRoom.TextColor = Colors.Black;
                        m_selectedRoom.BackgroundColor = Colors.White;
                    }
                    m_selectedRoom = value;
                    if (m_selectedRoom is not null)
                    {
                        m_selectedRoom.TextColor = Colors.White;
                        m_selectedRoom.BackgroundColor = Colors.Black;
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

        public LobbyMainPageViewModel(IClientHubService clientHubService, INavigationService navigationService)
        {
            m_clientHubService = clientHubService;
            m_navigationService = navigationService;
            m_createLobbyResponseMessageHandlerDelegate = new LobbyResponseMessageHandlerDelegate<CreateLobbyResponseMessage>(OnCreateLobbyResponseMessageReceived);
            m_joinLobbyResponseMessageHandlerDelegate = new LobbyResponseMessageHandlerDelegate<JoinLobbyResponseMessage>(OnJoinLobbyResponseMessageReceived);
            m_startMatchmakingResponseMessageHandlerDelegate = new LobbyResponseMessageHandlerDelegate<StartMatchmakingResponseMessage>(OnStartMatchmakingResponseMessageReceived);
            m_stopMatchmakingResponseMessageHandlerDelegate = new LobbyResponseMessageHandlerDelegate<StopMatchmakingResponseMessage>(OnStopMatchmakingResponseMessageReceived);
            Rooms = new ObservableCollection<RoomModel>();

            JoinGameCommand = new Command(JoinGame, () => SelectedRoom != null);

            CreateGameCommand = new Command(CreateGame);

            SelectFriendlyModeCommand = new Command(SelectFriendlyMode);

            SelectCompetitiveModeCommand = new Command(SelectCompetitiveMode);

            StartSearchCommand = new Command(StartSearch);
            StopSearchCommand = new Command(StopSearch);
            LogoutCommand = new Command(Logout);
        }

        private void Logout()
        {
            throw new NotImplementedException();
        }

        public void Register(IMessageRegisterer registerer)
        {
            registerer.Register(m_createLobbyResponseMessageHandlerDelegate);
            registerer.Register(m_joinLobbyResponseMessageHandlerDelegate);
            registerer.Register(m_startMatchmakingResponseMessageHandlerDelegate);
            registerer.Register(m_stopMatchmakingResponseMessageHandlerDelegate);
        }

        public void Unregister(IMessageRegisterer registerer)
        {
            registerer.Unregister(m_createLobbyResponseMessageHandlerDelegate);
            registerer.Unregister(m_joinLobbyResponseMessageHandlerDelegate);
            registerer.Unregister(m_startMatchmakingResponseMessageHandlerDelegate);
            registerer.Unregister(m_stopMatchmakingResponseMessageHandlerDelegate);
        }

        private void CreateGame()
        {
            throw new NotImplementedException();
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

        private Task<bool> OnStartMatchmakingResponseMessageReceived(StartMatchmakingResponseMessage message)
        {
            if (message.Success)
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
                return Task.FromResult(true);
            }

            return Task.FromResult(false);
        }

        private Task<bool> OnStopMatchmakingResponseMessageReceived(StopMatchmakingResponseMessage message)
        {
            if (message.Success)
            {
                _timer?.Stop();
                _timer?.Dispose();
                IsSearching = false;
                return Task.FromResult(true);
            }

            return Task.FromResult(false);
        }

        private async Task<bool> OnJoinLobbyResponseMessageReceived(JoinLobbyResponseMessage message)
        {
            if (message.Success)
            {
                await m_navigationService.NavigateToAsync("//LobbyPage", new Dictionary<string, object>
                {
                    { "LobbyDto", message.LobbyDto }
                });
                return true;
            }
            return false;
        }

        private async Task<bool> OnCreateLobbyResponseMessageReceived(CreateLobbyResponseMessage message)
        {
            if (message.Success)
            {
                await m_navigationService.NavigateToAsync("//LobbyPage", new Dictionary<string, object>
                {
                    { "LobbyDto", message.LobbyDto }
                });
                return true;
            }
            return false;
        }

        private async void StartSearch()
        {
            await m_clientHubService.InvokeLobbyCommand(new StartMatchmakingRequestMessage());
        }

        private async void StopSearch()
        {
            await m_clientHubService.InvokeLobbyCommand(new StopMatchmakingRequestMessage());
        }

        private readonly LobbyResponseMessageHandlerDelegate<CreateLobbyResponseMessage> m_createLobbyResponseMessageHandlerDelegate;
        private readonly LobbyResponseMessageHandlerDelegate<JoinLobbyResponseMessage> m_joinLobbyResponseMessageHandlerDelegate;
        private readonly LobbyResponseMessageHandlerDelegate<StartMatchmakingResponseMessage> m_startMatchmakingResponseMessageHandlerDelegate;
        private readonly LobbyResponseMessageHandlerDelegate<StopMatchmakingResponseMessage> m_stopMatchmakingResponseMessageHandlerDelegate;
        private readonly IClientHubService m_clientHubService;
        private readonly INavigationService m_navigationService;
    }
}