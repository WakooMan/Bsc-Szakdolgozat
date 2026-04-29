using SevenWondersUI.Services;
using System.Windows.Input;

namespace SevenWondersUI.ViewModels
{
    public class PlayerNamePageViewModel: BaseViewModel
    {
        public PlayerNamePageViewModel(INavigationService navigationService)
        {
            m_navigationService = navigationService;
            m_player1Entry = ("1. játékos neve:", string.Empty);
            m_player2Entry = ("2. játékos neve:", string.Empty);
            m_startText = "Indítás";
            m_backText = "Vissza";
            m_startCommand = new Command(OnStart, ValidatePlayerNames);
            BackCommand = new Command(OnBack, () => true);
        }

        public string Player1NameText
        {
            get 
            {
                return m_player1Entry.labelText;
            }
            set
            {
                if (m_player1Entry.labelText != value)
                {
                    m_player1Entry.labelText = value;
                    OnPropertyChanged();
                }
            }
        }

        public string Player1Name
        {
            get
            {
                return m_player1Entry.entryText;
            }
            set
            {
                if (m_player1Entry.entryText != value)
                {
                    m_player1Entry.entryText = value;
                    OnPropertyChanged();
                    m_startCommand.ChangeCanExecute();
                }
            }
        }

        public string Player2NameText
        {
            get
            {
                return m_player2Entry.labelText;
            }
            set
            {
                if (m_player2Entry.labelText != value)
                {
                    m_player2Entry.labelText = value;
                    OnPropertyChanged();
                }
            }
        }

        public string Player2Name
        {
            get
            {
                return m_player2Entry.entryText;
            }
            set
            {
                if (m_player2Entry.entryText != value)
                {
                    m_player2Entry.entryText = value;
                    OnPropertyChanged();
                    m_startCommand.ChangeCanExecute();
                }
            }
        }

        public string StartText
        {
            get
            {
                return m_startText;
            }
            set
            {
                if (m_startText != value)
                {
                    m_startText = value;
                    OnPropertyChanged();
                }
            }
        }

        public string BackText
        {
            get
            {
                return m_backText;
            }
            set
            {
                if (m_backText != value)
                {
                    m_backText = value;
                    OnPropertyChanged();
                }
            }
        }

        public ICommand StartCommand => m_startCommand;
        public ICommand BackCommand { get; }

        private bool ValidatePlayerNames()
        {
            return ValidatePlayerName(m_player1Entry.entryText) && ValidatePlayerName(m_player2Entry.entryText) && m_player1Entry.entryText != m_player2Entry.entryText;
        }

        private bool ValidatePlayerName(string playerName)
        {
            return !string.IsNullOrEmpty(playerName) && playerName.All(char.IsLetterOrDigit) && playerName.Length >= 2 && playerName.Length <= 6;
        }

        private async void OnStart()
        {
            await m_navigationService.NavigateToAsync("//PlayerVSPlayerGamePage", new Dictionary<string, object>
            { 
                { "Player1Name", m_player1Entry.entryText },
                { "Player2Name", m_player2Entry.entryText }
            });
            m_player1Entry.entryText = string.Empty;
            m_player2Entry.entryText = string.Empty;
        }

        private async void OnBack()
        {
            await m_navigationService.NavigateToAsync("//SingleplayerModePage");
            m_player1Entry.entryText = string.Empty;
            m_player2Entry.entryText = string.Empty;
        }


        private (string labelText, string entryText) m_player1Entry;
        private (string labelText, string entryText) m_player2Entry;
        private string m_startText;
        private string m_backText;
        private readonly INavigationService m_navigationService;
        private readonly Command m_startCommand;
    }
}
