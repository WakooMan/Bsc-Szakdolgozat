using CommunityToolkit.Maui.Views;
using SevenWonders.SceneEditor.ViewModels;

namespace SevenWonders.SceneEditor.Views;

public partial class AddLayerPopupWindow : Popup
{
    public AddPopupWindowViewModel ViewModel => m_viewModel;

	public AddLayerPopupWindow(AddPopupWindowViewModel viewModel)
	{
		InitializeComponent();
		m_viewModel = viewModel;
		BindingContext = m_viewModel;
    }

    private void Button_Clicked(object sender, EventArgs e)
    {
        Close();
    }

    private readonly AddPopupWindowViewModel m_viewModel;
}