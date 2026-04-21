using SevenWondersUI.Services;
using System.Windows.Input;

namespace SevenWondersUI.ViewModels
{
    public class AIDifficultyPageViewModel: BaseViewModel
    {
        public AIDifficultyPageViewModel(INavigationService navigationService)
        {
            m_navigationService = navigationService;
            m_title = "Nehézségi szint";
            m_easyButton = ("Könnyű", true);
            m_mediumButton = ("Közepes", false);
            m_hardButton = ("Nehéz", false);
            m_backButton = ("Vissza", true);
            EasyCommand = new Command(OnEasyClicked, () => m_easyButton.isEnabled);
            MediumCommand = new Command(OnMediumClicked, () => m_mediumButton.isEnabled);
            HardCommand = new Command(OnHardClicked, () => m_hardButton.isEnabled);
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

        public string EasyText
        {
            get
            {
                return m_easyButton.text;
            }
            set
            {
                if (m_easyButton.text != value)
                {
                    m_easyButton.text = value;
                    OnPropertyChanged();
                }
            }
        }

        public string MediumText
        {
            get
            {
                return m_mediumButton.text;
            }
            set
            {
                if (m_mediumButton.text != value)
                {
                    m_mediumButton.text = value;
                    OnPropertyChanged();
                }
            }
        }

        public string HardText
        {
            get
            {
                return m_hardButton.text;
            }
            set
            {
                if (m_hardButton.text != value)
                {
                    m_hardButton.text = value;
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

        public ICommand EasyCommand { get; }
        public ICommand MediumCommand { get; }
        public ICommand HardCommand { get; }
        public ICommand BackCommand { get; }

        private async void OnBackClicked()
        {
            await m_navigationService.NavigateToAsync("//SingleplayerModePage");
        }

        private async void OnHardClicked()
        {
            await m_navigationService.NavigateToAsync("//GamePage");
        }

        private async void OnMediumClicked()
        {
            await m_navigationService.NavigateToAsync("//GamePage");
        }

        private async void OnEasyClicked()
        {
            await m_navigationService.NavigateToAsync("//GamePage");
        }

        private string m_title;
        private (string text, bool isEnabled) m_easyButton;
        private (string text, bool isEnabled) m_mediumButton;
        private (string text, bool isEnabled) m_hardButton;
        private (string text, bool isEnabled) m_backButton;
        private readonly INavigationService m_navigationService;
    }
}
