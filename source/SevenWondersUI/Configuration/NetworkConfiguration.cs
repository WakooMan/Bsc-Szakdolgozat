using SevenWonders.WebClient.Model.Services;

namespace SevenWondersUI.Configuration
{
    public class NetworkConfiguration : INetworkConfiguration
    {

        public Uri ApiBaseUri { get; private set; }
        public Uri SignalRHubUri { get; private set; }

        public TimeSpan HttpTimeout { get; private set; }
        public TimeSpan ServerTimeout { get; private set; }
        public TimeSpan HandshakeTimeout { get; private set; }
        public bool UseHttps => ApiBaseUri?.Scheme == Uri.UriSchemeHttps;

        public NetworkConfiguration(IAppConfiguration appConfiguration)
        {
            m_appConfiguration = appConfiguration ?? throw new ArgumentNullException(nameof(appConfiguration), "AppConfiguration cannot be null");
            ApiBaseUri = null!;
            SignalRHubUri = null!;
        }

        public void LoadConfiguration()
        {
            if (m_appConfiguration.AppConfig is null)
            {
                throw new ArgumentNullException(nameof(m_appConfiguration.AppConfig), "AppConfig cannot be null");
            }

            ApiBaseUri = new Uri(m_appConfiguration.AppConfig.Configuration.AppSettings.ApiBaseUri);
            SignalRHubUri = new Uri(ApiBaseUri, m_appConfiguration.AppConfig.Configuration.AppSettings.SignalRHubUri);
            HttpTimeout = TimeSpan.FromSeconds(m_appConfiguration.AppConfig.Configuration.AppSettings.HttpTimeout);
            ServerTimeout = TimeSpan.FromSeconds(m_appConfiguration.AppConfig.Configuration.AppSettings.ServerTimeout);
            HandshakeTimeout = TimeSpan.FromSeconds(m_appConfiguration.AppConfig.Configuration.AppSettings.HandshakeTimeout);
        }

        private readonly IAppConfiguration m_appConfiguration;
    }
}
