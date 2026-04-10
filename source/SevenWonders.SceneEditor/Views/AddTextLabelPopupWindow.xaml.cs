using CommunityToolkit.Maui.Views;
using SevenWonders.SceneEditor.ViewModels;

namespace SevenWonders.SceneEditor.Views;

public partial class AddTextLabelPopupWindow : Popup
{
    public AddTextLabelPopupWindowViewModel ViewModel => m_viewModel;

    public AddTextLabelPopupWindow(AddTextLabelPopupWindowViewModel viewModel)
    {
        InitializeComponent();
        m_viewModel = viewModel;
        BindingContext = m_viewModel;
    }

    private void Button_Clicked(object sender, EventArgs e)
    {
        Close();
    }

    private readonly AddTextLabelPopupWindowViewModel m_viewModel;
}
