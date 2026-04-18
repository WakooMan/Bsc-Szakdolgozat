namespace SevenWondersUI.Configuration
{
    public class ConfigurationData
    {
        public AppSettingsData AppSettings { get; set; }
        public ConfigurationData() { AppSettings = new AppSettingsData(); }
    }
}
