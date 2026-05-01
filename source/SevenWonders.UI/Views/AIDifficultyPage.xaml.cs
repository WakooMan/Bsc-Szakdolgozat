using SevenWonders.UI.ViewModels;

namespace SevenWonders.UI.Views;

public partial class AIDifficultyPage : ContentPage
{
	public AIDifficultyPage(AIDifficultyPageViewModel aIDifficultyPageViewModel)
	{
		InitializeComponent();
		BindingContext = aIDifficultyPageViewModel;
    }
}