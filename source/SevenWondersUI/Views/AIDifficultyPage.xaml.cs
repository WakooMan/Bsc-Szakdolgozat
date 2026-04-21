using SevenWondersUI.ViewModels;

namespace SevenWondersUI.Views;

public partial class AIDifficultyPage : ContentPage
{
	public AIDifficultyPage(AIDifficultyPageViewModel aIDifficultyPageViewModel)
	{
		InitializeComponent();
		BindingContext = aIDifficultyPageViewModel;
    }
}