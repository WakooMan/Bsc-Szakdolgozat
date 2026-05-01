using System.Text.Json;

namespace SevenWonders.UI.Configuration
{
    public class AppConfiguration : IAppConfiguration
    {
        public AppConfig? AppConfig { get; private set; }

        public async Task LoadConfig()
        {
            using var stream = await FileSystem.OpenAppPackageFileAsync("appsettings.json");
            using var reader = new StreamReader(stream);
            var contents = await reader.ReadToEndAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            AppConfig = JsonSerializer.Deserialize<AppConfig>(contents, options);
        }
    }
}
