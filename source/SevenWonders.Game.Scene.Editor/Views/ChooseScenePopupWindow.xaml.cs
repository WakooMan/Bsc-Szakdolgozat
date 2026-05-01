using CommunityToolkit.Maui.Views;
using SevenWonders.Game.Scene.Editor.ViewModels;

namespace SevenWonders.Game.Scene.Editor.Views;

public partial class ChooseScenePopupWindow : Popup
{
    public ChooseScenePopupWindowViewModel ViewModel => m_viewModel;

    public ChooseScenePopupWindow(ChooseScenePopupWindowViewModel viewModel)
	{
		InitializeComponent();
        m_viewModel = viewModel;
        BindingContext = m_viewModel;
	}
    private void Button_Clicked(object sender, EventArgs e)
    {
        Close();
    }

    private readonly ChooseScenePopupWindowViewModel m_viewModel;
}