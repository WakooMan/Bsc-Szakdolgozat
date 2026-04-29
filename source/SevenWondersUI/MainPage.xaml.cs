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

        protected override void OnAppearing()
        {
            base.OnAppearing();
            InitializeComponent();
        }
    }

}
