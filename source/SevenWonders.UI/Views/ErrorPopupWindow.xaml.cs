using CommunityToolkit.Maui.Views;
using SevenWonders.UI.ViewModels;

namespace SevenWonders.UI.Views;

public partial class ErrorPopupWindow : Popup
{
    public ErrorPopupWindow(ErrorPopupViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

    private void OkButton_Clicked(object sender, EventArgs e)
    {
        Close();
    }
}
