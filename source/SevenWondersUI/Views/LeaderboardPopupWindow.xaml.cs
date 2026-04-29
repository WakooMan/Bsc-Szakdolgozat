using CommunityToolkit.Maui.Views;
using SevenWondersUI.ViewModels;

namespace SevenWondersUI.Views;

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
