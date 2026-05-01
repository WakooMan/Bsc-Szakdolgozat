using SevenWonders.UI.Services;
using System.Windows.Input;

namespace SevenWonders.UI.ViewModels
{
    public class SingleplayerModePageViewModel: BaseViewModel
    {
        public SingleplayerModePageViewModel(INavigationService navigationService)
        {
            m_navigationService = navigationService;
            m_title = "Játékmód választása";
            m_playerVsPlayerButton = ("Játékos VS Játékos", true);
            m_playerVsAIButton = ("Játékos VS MI", true);
            m_backButton = ("Vissza", true);
            PlayerVsPlayerCommand = new Command(OnPlayerVsPlayerClicked, () => m_playerVsPlayerButton.isEnabled);
            PlayerVsAICommand = new Command(OnPlayerVsAIClicked, () => m_playerVsAIButton.isEnabled);
            BackCommand = new Command(OnBackClicked, () => m_backButton.isEnabled);
        }

        public string Title
        {
            get
            {
                return m_title;
            }
            set
            {
                if (m_title != value)
                {
                    m_title = value;
                    OnPropertyChanged();
                }
            }
        }

        public string PlayerVsPlayerText
        {
            get
            {
                return m_playerVsPlayerButton.text;
            }
            set
            {
                if (m_playerVsPlayerButton.text != value)
                {
                    m_playerVsPlayerButton.text = value;
                    OnPropertyChanged();
                }
            }
        }

        public string PlayerVsAIText
        {
            get
            {
                return m_playerVsAIButton.text;
            }
            set
            {
                if (m_playerVsAIButton.text != value)
                {
                    m_playerVsAIButton.text = value;
                    OnPropertyChanged();
                }
            }
        }

        public string BackText
        {
            get
            {
                return m_backButton.text;
            }
            set
            {
                if (m_backButton.text != value)
                {
                    m_backButton.text = value;
                    OnPropertyChanged();
                }
            }
        }

        public ICommand PlayerVsPlayerCommand { get; }
        public ICommand PlayerVsAICommand { get; }
        public ICommand BackCommand { get; }

        private async void OnBackClicked()
        {
            await m_navigationService.NavigateToAsync("//MainPage");
        }

        private async void OnPlayerVsAIClicked()
        {
            await m_navigationService.NavigateToAsync("//AIDifficultyPage");
        }

        private async void OnPlayerVsPlayerClicked()
        {
            await m_navigationService.NavigateToAsync("//PlayerNamePage");
        }

        private string m_title;
        private (string text, bool isEnabled) m_playerVsPlayerButton;
        private (string text, bool isEnabled) m_playerVsAIButton;
        private (string text, bool isEnabled) m_backButton;
        private readonly INavigationService m_navigationService;
    }
}
