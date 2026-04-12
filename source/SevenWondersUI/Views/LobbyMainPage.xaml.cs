using SevenWondersUI.ViewModels;

namespace SevenWondersUI.Views;

public partial class LobbyMainPage : ContentPage
{
	public LobbyMainPage(LobbyMainPageViewModel lobbyMainPageViewModel)
	{
		InitializeComponent();
		BindingContext = lobbyMainPageViewModel;
	}
}