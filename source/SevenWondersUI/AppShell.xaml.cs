using SevenWondersUI.Services;

namespace SevenWondersUI
{
    public partial class AppShell : Shell
    {
        public AppShell(INavigationService navigationService)
        {
            m_navigationService = navigationService;
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            await m_navigationService.InitializeAsync();
        }

        private readonly INavigationService m_navigationService;
    }
}
