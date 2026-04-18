using SevenWonders.Common;
using SevenWondersUI.Configuration;
using SevenWondersUI.ViewModels;
using System.Text.Json;

namespace SevenWondersUI
{
    public partial class MainPage : ContentPage
    {
        public MainPage(MainPageViewModel mainPageViewModel)
        {
            BindingContext = mainPageViewModel;
        }
        protected override async void OnAppearing()
        {
            base.OnAppearing();

            InitializeComponent();
            using var stream = await FileSystem.OpenAppPackageFileAsync("appsettings.json");
            using var reader = new StreamReader(stream);
            var contents = await reader.ReadToEndAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            var config = JsonSerializer.Deserialize<AppConfig>(contents, options);
            if (config is not null)
            {
                GameLog.InitializeFileLogger(FileSystem.AppDataDirectory, config.Configuration.AppSettings.LogFileName);
            }
        }
    }

}
