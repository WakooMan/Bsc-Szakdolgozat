using SevenWondersUI.ViewModels;

namespace SevenWondersUI.Views;

public partial class PlayerNamePage : ContentPage
{
	public PlayerNamePage(PlayerNamePageViewModel playerNamePageViewModel)
	{
		InitializeComponent();
		BindingContext = playerNamePageViewModel;
	}
}