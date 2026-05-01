using SevenWonders.UI.ViewModels;

namespace SevenWonders.UI.Views;

public partial class SingleplayerModePage : ContentPage
{
	public SingleplayerModePage(SingleplayerModePageViewModel singleplayerModePageViewModel)
	{
		InitializeComponent();
		BindingContext = singleplayerModePageViewModel;
    }
}