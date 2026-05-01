using SevenWonders.UI.ViewModels;

namespace SevenWonders.UI.Views;

public partial class PlayerNamePage : ContentPage
{
	public PlayerNamePage(PlayerNamePageViewModel playerNamePageViewModel)
	{
		InitializeComponent();
		BindingContext = playerNamePageViewModel;
	}
}