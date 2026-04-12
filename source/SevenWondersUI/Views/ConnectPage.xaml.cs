using SevenWondersUI.ViewModels;

namespace SevenWondersUI.Views;

public partial class ConnectPage : ContentPage
{
	public ConnectPage(ConnectPageViewModel connectPageViewModel)
	{
		InitializeComponent();
        m_connectPageViewModel = connectPageViewModel;
        BindingContext = m_connectPageViewModel;
	}

    protected override void OnAppearing()
    {
        base.OnAppearing();
        m_connectPageViewModel.ConnectToServer().ConfigureAwait(false);
    }

    private readonly ConnectPageViewModel m_connectPageViewModel;
}