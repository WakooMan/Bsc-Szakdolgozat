namespace SevenWonders.UI.Configuration
{
    public interface IAppConfiguration
    {
        AppConfig? AppConfig { get; }

        Task LoadConfig();
    }
}
