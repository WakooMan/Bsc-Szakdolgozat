namespace SevenWonders.UI.Configuration
{
    public class AppSettingsData
    {
        public string LogFileName { get; set; }
        public string ApiBaseUri { get; set; }
        public string SignalRHubUri { get; set; }
        public int HttpTimeout { get; set; }
        public int ServerTimeout { get; set; }
        public int HandshakeTimeout { get; set; }
        public AppSettingsData()
        {
            LogFileName = string.Empty; 
            ApiBaseUri = string.Empty; 
            SignalRHubUri = string.Empty; 
        }
    }
}
