using SevenWondersUI.ViewModels;

namespace SevenWondersUI.Views;

public partial class SingleplayerModePage : ContentPage
{
	public SingleplayerModePage(SingleplayerModePageViewModel singleplayerModePageViewModel)
	{
		InitializeComponent();
		BindingContext = singleplayerModePageViewModel;
    }
}