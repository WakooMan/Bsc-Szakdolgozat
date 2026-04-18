using CommunityToolkit.Maui.Views;
using SevenWondersUI.ViewModels;

namespace SevenWondersUI.Views;

public partial class CreateGamePopupWindow : Popup
{
    public CreateGamePopupViewModel ViewModel => m_viewModel;

    public CreateGamePopupWindow(CreateGamePopupViewModel viewModel)
    {
        InitializeComponent();
        m_viewModel = viewModel;
        BindingContext = m_viewModel;
    }

    private void Button_Clicked(object sender, EventArgs e)
    {
        Close();
    }

    private readonly CreateGamePopupViewModel m_viewModel;
}
