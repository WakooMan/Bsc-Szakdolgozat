using System.Collections.ObjectModel;
using System.Windows.Input;

namespace SevenWondersUI.ViewModels
{
    public class LobbyMainPageViewModel : BaseViewModel
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

        public ObservableCollection<string> Rooms { get; set; }

        public ICommand SelectFriendlyModeCommand { get; }
        public ICommand SelectCompetitiveModeCommand { get; }
        public ICommand StartSearchCommand { get; }
        public ICommand StopSearchCommand { get; }
        public ICommand LogoutCommand { get; }

        public LobbyMainPageViewModel()
        {
            Rooms = new ObservableCollection<string> { "Szoba #1", "Szoba #2", "Szoba #3" };

            SelectFriendlyModeCommand = new Command(() => {
                IsFriendlyModeVisible = true;
                IsCompetitiveModeVisible = false;
            });

            SelectCompetitiveModeCommand = new Command(() => {
                IsFriendlyModeVisible = false;
                IsCompetitiveModeVisible = true;
            });

            StartSearchCommand = new Command(StartSearch);
            StopSearchCommand = new Command(StopSearch);
            LogoutCommand = new Command(async () => await App.Current.MainPage.DisplayAlert("Kijelentkezés", "Sikeres kijelentkezés!", "OK"));
        }

        private void StartSearch()
        {
            IsSearching = true;
            _secondsElapsed = 0;
            SearchTimerText = "00:00";

            _timer = new System.Timers.Timer(1000);
            _timer.Elapsed += (s, e) => {
                _secondsElapsed++;
                var time = TimeSpan.FromSeconds(_secondsElapsed);
                SearchTimerText = time.ToString(@"mm\:ss");
            };
            _timer.Start();
        }

        private void StopSearch()
        {
            _timer?.Stop();
            _timer?.Dispose();
            IsSearching = false;
        }
    }
}