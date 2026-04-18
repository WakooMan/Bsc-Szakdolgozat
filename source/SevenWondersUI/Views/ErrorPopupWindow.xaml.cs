using CommunityToolkit.Maui.Views;
using SevenWondersUI.ViewModels;

namespace SevenWondersUI.Views;

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
