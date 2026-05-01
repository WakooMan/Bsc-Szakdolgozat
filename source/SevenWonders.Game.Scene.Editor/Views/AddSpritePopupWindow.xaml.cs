using CommunityToolkit.Maui.Views;
using SevenWonders.Game.Scene.Editor.ViewModels;

namespace SevenWonders.Game.Scene.Editor.Views;

public partial class AddSpritePopupWindow : Popup
{
    public AddSpritePopupWindowViewModel ViewModel => m_viewModel;
    public AddSpritePopupWindow(AddSpritePopupWindowViewModel addSpritePopupWindowViewModel)
	{
		InitializeComponent();
		m_viewModel = addSpritePopupWindowViewModel;
        BindingContext = m_viewModel;
	}

    private void Button_Clicked(object sender, EventArgs e)
    {
        Close();
    }

    private readonly AddSpritePopupWindowViewModel m_viewModel;
}