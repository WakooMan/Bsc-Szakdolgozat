using SevenWonders.AI.Model.AIModelHandler;
using SevenWonders.AI.Model.Cache;
using SevenWonders.Common;
using SevenWondersUI.Services;
using System.Windows.Input;

namespace SevenWondersUI.ViewModels
{
    public class AIDifficultyPageViewModel: BaseViewModel
    {
        public AIDifficultyPageViewModel(INavigationService navigationService, IAIModelHandlerCache aIModelHandlerCache)
        {
            m_navigationService = navigationService;
            m_aiModelHandlerCache = aIModelHandlerCache;
            m_title = "Nehézségi szint";
            m_easyButton = ("Könnyű", true);
            m_mediumButton = ("Közepes", true);
            m_hardButton = ("Nehéz", true);
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
            m_aiModelHandlerCache.HardAIModelHandler.LoadModel(AIModelType.Hard);
            await m_navigationService.NavigateToAsync("//PlayerVSAIGamePage", new Dictionary<string, object>
            {
                { "Player1Name", "Player" },
                { "Player2Name", "HardAI" },
                { "Player2Type", PlayerType.HardAI }
            });
        }

        private async void OnMediumClicked()
        {
            m_aiModelHandlerCache.MediumAIModelHandler.LoadModel(AIModelType.Medium);
            await m_navigationService.NavigateToAsync("//PlayerVSAIGamePage", new Dictionary<string, object>
            {
                { "Player1Name", "Player" },
                { "Player2Name", "MediumAI" },
                { "Player2Type", PlayerType.MediumAI }
            });
        }

        private async void OnEasyClicked()
        {
            m_aiModelHandlerCache.EasyAIModelHandler.LoadModel(AIModelType.Easy);
            await m_navigationService.NavigateToAsync("//PlayerVSAIGamePage", new Dictionary<string, object>
            {
                { "Player1Name", "Player" },
                { "Player2Name", "EasyAI" },
                { "Player2Type", PlayerType.EasyAI }
            });
        }

        private string m_title;
        private (string text, bool isEnabled) m_easyButton;
        private (string text, bool isEnabled) m_mediumButton;
        private (string text, bool isEnabled) m_hardButton;
        private (string text, bool isEnabled) m_backButton;
        private readonly INavigationService m_navigationService;
        private readonly IAIModelHandlerCache m_aiModelHandlerCache;
    }
}
