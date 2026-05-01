using CommunityToolkit.Maui.Views;
using SevenWonders.UI.ViewModels;

namespace SevenWonders.UI.Views;

public partial class LeaderboardPopupWindow : Popup
{
    public LeaderboardPopupWindow(LeaderboardPopupViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

    private void CloseButton_Clicked(object sender, EventArgs e)
    {
        Close();
    }
}
