namespace SevenWondersUI.Configuration
{
    public interface IAppConfiguration
    {
        AppConfig? AppConfig { get; }

        Task LoadConfig();
    }
}
