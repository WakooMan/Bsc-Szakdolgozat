using SevenWonders.Common;
using SevenWondersUI.ViewModels;

namespace SevenWondersUI
{
    public partial class MainPage : ContentPage
    {
        public MainPage(MainPageViewModel mainPageViewModel)
        {
            BindingContext = mainPageViewModel;
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();

            InitializeComponent();
            GameLog.InitializeFileLogger(FileSystem.AppDataDirectory);
        }
    }

}
