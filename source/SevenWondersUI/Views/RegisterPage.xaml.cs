using SevenWondersUI.ViewModels;

namespace SevenWondersUI.Views;

public partial class RegisterPage : ContentPage
{
	public RegisterPage(RegisterPageViewModel registerPageViewModel)
	{
		InitializeComponent();
		BindingContext = registerPageViewModel;
	}
}