using SevenWonders.UI.Services;
using System.Windows.Input;

namespace SevenWonders.UI.ViewModels
{
    public class MainPageViewModel : BaseViewModel
    {
        public MainPageViewModel(INavigationService navigationService, IExitService exitService)
        {
            m_navigationService = navigationService;
            m_exitService = exitService;
            m_title = "7 Csoda Párbaj";
            m_singleplayerButton = ("Egyjátékos mód", true);
            m_multiplayerButton = ("Többjátékos mód", true);
            m_aILearningButton = ("MI Tanulás", false);
            m_exitButton = ("Kilépés", true);
            SinglePlayerCommand = new Command(OnSinglePlayerClicked, () => m_singleplayerButton.isEnabled);
            MultiplayerCommand = new Command(OnMultiplayerClicked, () => m_multiplayerButton.isEnabled);
            AILearningCommand = new Command(OnAILearningClicked, () => m_aILearningButton.isEnabled);
            ExitCommand = new Command(OnExitClicked, () => m_exitButton.isEnabled);
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

        public string SingleplayerText
        {
            get
            {
                return m_singleplayerButton.text;
            }
            set
            {
                if (m_singleplayerButton.text != value)
                {
                    m_singleplayerButton.text = value;
                    OnPropertyChanged();
                }
            }
        }

        public string MultiplayerText
        {
            get
            {
                return m_multiplayerButton.text;
            }
            set
            {
                if (m_multiplayerButton.text != value)
                {
                    m_multiplayerButton.text = value;
                    OnPropertyChanged();
                }
            }
        }

        public string AILearningText
        {
            get
            {
                return m_aILearningButton.text;
            }
            set
            {
                if (m_aILearningButton.text != value)
                {
                    m_aILearningButton.text = value;
                    OnPropertyChanged();
                }
            }
        }

        public string ExitText
        {
            get
            {
                return m_exitButton.text;
            }
            set
            {
                if (m_exitButton.text != value)
                {
                    m_exitButton.text = value;
                    OnPropertyChanged();
                }
            }
        }

        public ICommand SinglePlayerCommand { get; }
        public ICommand MultiplayerCommand { get; }
        public ICommand AILearningCommand { get; }
        public ICommand ExitCommand { get; }

        private void OnExitClicked()
        {
            m_exitService.ExitApplication();
        }

        private void OnAILearningClicked()
        {
            throw new NotImplementedException();
        }

        private async void OnMultiplayerClicked()
        {
            await m_navigationService.NavigateToAsync("//LoginPage");
        }

        private async void OnSinglePlayerClicked()
        {
            await m_navigationService.NavigateToAsync("//SingleplayerModePage");
        }

        private string m_title;
        private (string text, bool isEnabled) m_singleplayerButton;
        private (string text, bool isEnabled) m_multiplayerButton;
        private (string text, bool isEnabled) m_aILearningButton;
        private (string text, bool isEnabled) m_exitButton;
        private readonly INavigationService m_navigationService;
        private readonly IExitService m_exitService;
    }
}
