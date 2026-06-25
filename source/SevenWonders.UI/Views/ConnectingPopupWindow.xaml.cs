using CommunityToolkit.Maui.Views;
using SevenWondersUI.ViewModels;

namespace SevenWondersUI.Views;

public partial class ConnectingPopupWindow : Popup
{
    public ConnectingPopupViewModel ViewModel => m_viewModel;

	public ConnectingPopupWindow(ConnectingPopupViewModel connectingPopupViewModel)
	{
		InitializeComponent();
        m_viewModel = connectingPopupViewModel;
        BindingContext = m_viewModel;
	}

    private void Cancel_Clicked(object sender, EventArgs e)
    {
        Close();
    }

    private readonly ConnectingPopupViewModel m_viewModel;
}