namespace SevenWonders.Web.Client.Model.Services
{
    public interface INetworkConfiguration
    {
        Uri ApiBaseUri { get; }
        Uri SignalRHubUri { get; }
        TimeSpan HttpTimeout { get; }
        TimeSpan ServerTimeout { get; }
        TimeSpan HandshakeTimeout { get; }
        bool UseHttps { get; }

        void LoadConfiguration();
    }
}
