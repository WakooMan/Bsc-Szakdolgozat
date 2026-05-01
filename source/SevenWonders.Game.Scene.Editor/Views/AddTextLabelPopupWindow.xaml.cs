using CommunityToolkit.Maui.Views;
using SevenWonders.Game.Scene.Editor.ViewModels;

namespace SevenWonders.Game.Scene.Editor.Views;

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
