using SevenWonders.Web.Client.Model;
using SevenWonders.UI.ViewModels;

namespace SevenWonders.UI.Views;

public partial class LobbyPage : ContentPage
{
	public LobbyPage(LobbyPageViewModel lobbyPageViewModel, IClientMessageDispatcher clientMessageDispatcher)
	{
		InitializeComponent();
		m_clientMessageDispatcher = clientMessageDispatcher;
		m_lobbyPageViewModel = lobbyPageViewModel;
        BindingContext = m_lobbyPageViewModel;
        m_lobbyPageViewModel.ChatMessages.CollectionChanged += (_, _) =>
		{
			if (lobbyPageViewModel.ChatMessages.Count > 0)
			{
				ChatCollectionView.ScrollTo(lobbyPageViewModel.ChatMessages[^1], animate: false);
			}
		};
	}

    protected override void OnAppearing()
    {
        base.OnAppearing();
        m_clientMessageDispatcher.RegisterHandler(m_lobbyPageViewModel);
    }
    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        m_clientMessageDispatcher.UnregisterHandler(m_lobbyPageViewModel);
    }

    private readonly IClientMessageDispatcher m_clientMessageDispatcher;
	private readonly LobbyPageViewModel m_lobbyPageViewModel;
}
