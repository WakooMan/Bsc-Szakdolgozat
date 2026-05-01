using SevenWonders.UI.ViewModels;

namespace SevenWonders.UI.Views;

public partial class RegisterPage : ContentPage
{
	public RegisterPage(RegisterPageViewModel registerPageViewModel)
	{
		InitializeComponent();
		BindingContext = registerPageViewModel;
	}
}