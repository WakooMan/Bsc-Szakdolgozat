using CommunityToolkit.Maui.Views;
using SevenWonders.Game.Scene.Editor.ViewModels;

namespace SevenWonders.Game.Scene.Editor.Views;

public partial class AddButtonPopupWindow : Popup
{
    public AddButtonPopupWindowViewModel ViewModel => m_viewModel;

    public AddButtonPopupWindow(AddButtonPopupWindowViewModel viewModel)
    {
        InitializeComponent();
        m_viewModel = viewModel;
        BindingContext = m_viewModel;
    }

    private void Button_Clicked(object sender, EventArgs e)
    {
        Close();
    }

    private readonly AddButtonPopupWindowViewModel m_viewModel;
}
