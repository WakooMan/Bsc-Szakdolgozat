using SevenWonders.Web.Client.Model.Services;

namespace SevenWonders.UI.Configuration
{
    public class DebugNetworkConfiguration : INetworkConfiguration
    {
        public Uri ApiBaseUri { get; private set; }

        public Uri SignalRHubUri { get; private set; }

        public TimeSpan HttpTimeout { get; private set; }
        public TimeSpan ServerTimeout { get; private set; }

        public TimeSpan HandshakeTimeout { get; private set; }

        public bool UseHttps => ApiBaseUri?.Scheme == Uri.UriSchemeHttps;

        public DebugNetworkConfiguration()
        {
            ApiBaseUri = null!;
            SignalRHubUri = null!;
        }

        public void LoadConfiguration()
        {
            ApiBaseUri = new Uri("https://localhost:7206");
            SignalRHubUri = new Uri("https://localhost:7206/serverhub");
            HttpTimeout = TimeSpan.FromSeconds(30);
            ServerTimeout = TimeSpan.FromSeconds(30);
            HandshakeTimeout = TimeSpan.FromSeconds(15);
        }
    }
}
