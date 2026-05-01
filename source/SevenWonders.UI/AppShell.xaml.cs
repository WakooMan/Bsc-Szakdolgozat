using SevenWonders.Common;
using SevenWonders.Game.Presenter;
using SevenWonders.Web.Client.Model.Services;
using SevenWonders.UI.Configuration;
using SevenWonders.UI.Services;

namespace SevenWonders.UI
{
    public partial class AppShell : Shell
    {
        public AppShell(INavigationService navigationService, 
                        IGameHandler gameHandler, 
                        IAppConfiguration appConfiguration,
                        INetworkConfiguration networkConfiguration)
        {
            m_navigationService = navigationService;
            m_gameHandler = gameHandler;
            m_appConfiguration = appConfiguration;
            m_networkConfiguration = networkConfiguration;
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            await m_appConfiguration.LoadConfig();
            m_networkConfiguration.LoadConfiguration();
            var config = m_appConfiguration.AppConfig;
            if (config is not null)
            {
                GameLog.InitializeFileLogger(FileSystem.AppDataDirectory, config.Configuration.AppSettings.LogFileName);
            }

            await m_navigationService.InitializeAsync();
            m_gameHandler.InitializeEngine();
        }

        private readonly INavigationService m_navigationService;
        private readonly IGameHandler m_gameHandler;
        private readonly IAppConfiguration m_appConfiguration;
        private readonly INetworkConfiguration m_networkConfiguration;
    }
}
