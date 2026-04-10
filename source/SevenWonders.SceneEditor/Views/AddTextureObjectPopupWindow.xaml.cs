using CommunityToolkit.Maui.Views;
using SevenWonders.SceneEditor.ViewModels;

namespace SevenWonders.SceneEditor.Views;

public partial class AddTextureObjectPopupWindow : Popup
{
    public AddTextureObjectPopupWindowViewModel ViewModel => m_viewModel;

    public AddTextureObjectPopupWindow(AddTextureObjectPopupWindowViewModel viewModel)
    {
        InitializeComponent();
        m_viewModel = viewModel;
        BindingContext = m_viewModel;
    }

    private void Button_Clicked(object sender, EventArgs e)
    {
        Close();
    }

    private readonly AddTextureObjectPopupWindowViewModel m_viewModel;
}
