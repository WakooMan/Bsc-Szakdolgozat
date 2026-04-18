using SevenWonders.WebClient.Model;
using SevenWondersUI.ViewModels;

namespace SevenWondersUI.Views;

public partial class LobbyMainPage : ContentPage
{
	public LobbyMainPage(LobbyMainPageViewModel lobbyMainPageViewModel, IClientMessageDispatcher clientMessageDispatcher)
	{
		InitializeComponent();
        m_clientMessageDispatcher = clientMessageDispatcher;
        m_lobbyMainPageViewModel = lobbyMainPageViewModel;
        BindingContext = m_lobbyMainPageViewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        m_clientMessageDispatcher.RegisterHandler(m_lobbyMainPageViewModel);
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        m_clientMessageDispatcher.UnregisterHandler(m_lobbyMainPageViewModel);
    }

    private readonly IClientMessageDispatcher m_clientMessageDispatcher;
    private readonly LobbyMainPageViewModel m_lobbyMainPageViewModel;
}