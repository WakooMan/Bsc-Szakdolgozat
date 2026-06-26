using CommunityToolkit.Maui.Views;
using SevenWondersUI.ViewModels;

namespace SevenWonders.UI.Views;

public partial class ConnectingPopupWindow : Popup
{
    public ConnectingPopupViewModel ViewModel => m_viewModel;

	public ConnectingPopupWindow(ConnectingPopupViewModel connectingPopupViewModel)
	{
		InitializeComponent();
        m_viewModel = connectingPopupViewModel;
        BindingContext = m_viewModel;
        Opened += (s, e) => m_viewModel.OnOpened();
        m_viewModel.OnConnectionFinished += (s, e) => Close();
    }

    private readonly ConnectingPopupViewModel m_viewModel;
}