using SevenWonders.Common;
using SevenWonders.UI.Configuration;
using SevenWonders.UI.ViewModels;
using System.Text.Json;

namespace SevenWonders.UI
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
        }
    }

}
