using SevenWonders.WebClient.Model;
using SevenWondersUI.ViewModels;

namespace SevenWondersUI.Views;

public partial class ConnectPage : ContentPage
{
	public ConnectPage(ConnectPageViewModel connectPageViewModel, IClientMessageDispatcher clientMessageDispatcher)
	{
		InitializeComponent();
		m_connectPageViewModel = connectPageViewModel;
		m_clientMessageDispatcher = clientMessageDispatcher;
		BindingContext = m_connectPageViewModel;
	}

	protected override void OnAppearing()
	{
		base.OnAppearing();
		m_clientMessageDispatcher.RegisterHandler(m_connectPageViewModel);
		m_connectPageViewModel.ConnectToServer().ConfigureAwait(false);
	}

	protected override void OnDisappearing()
	{
		base.OnDisappearing();
		m_clientMessageDispatcher.UnregisterHandler(m_connectPageViewModel);
	}

	private readonly ConnectPageViewModel m_connectPageViewModel;
	private readonly IClientMessageDispatcher m_clientMessageDispatcher;
}