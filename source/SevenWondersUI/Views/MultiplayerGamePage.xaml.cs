using SevenWonders.WebClient.Model;
using SevenWondersUI.ViewModels;
using SkiaSharp.Views.Maui;
using System.Numerics;

namespace SevenWondersUI.Views;

public partial class MultiplayerGamePage : ContentPage
{
    public MultiplayerGamePage(MultiplayerGamePageViewModel multiplayerGamePageViewModel, IClientMessageDispatcher clientMessageDispatcher)
    {
        m_multiplayerGamePageViewModel = multiplayerGamePageViewModel;
        m_clientMessageDispatcher = clientMessageDispatcher;
        InitializeComponent();
        m_redrawRequested = (e, args) =>
        {
            if (m_gameView is not null)
            {
                m_gameView.InvalidateSurface();
            }
        };
        BindingContext = m_multiplayerGamePageViewModel;
    }
    protected override async void OnAppearing()
    {
        base.OnAppearing();
        while (m_gameView is null)
        {
            await Task.Delay(100);
        }
        m_clientMessageDispatcher.RegisterHandler(m_multiplayerGamePageViewModel);
        m_multiplayerGamePageViewModel.GameHandler.SubscribeRedrawRequested(m_redrawRequested);
        await m_multiplayerGamePageViewModel.Initialize();
        OnCanvasSizeChanged(this, EventArgs.Empty);
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        m_clientMessageDispatcher.UnregisterHandler(m_multiplayerGamePageViewModel);
        m_multiplayerGamePageViewModel.GameHandler.UnsubscribeRedrawRequested(m_redrawRequested);
    }

    private void OnCanvasSizeChanged(object sender, EventArgs e)
    {
        m_multiplayerGamePageViewModel.GameHandler.Resize(new Vector2((float)m_mainGrid.Width, (float)m_mainGrid.Height));
    }


    private void OnPaintSurface(object sender, SKPaintSurfaceEventArgs e)
    {
        m_multiplayerGamePageViewModel.GameHandler.Render(e.Surface.Canvas);
    }

    private void OnTouch(object sender, SKTouchEventArgs e)
    {
        m_multiplayerGamePageViewModel.GameHandler.OnTouchEvent(e);
    }

    private readonly MultiplayerGamePageViewModel m_multiplayerGamePageViewModel;
    private readonly IClientMessageDispatcher m_clientMessageDispatcher;
    private readonly EventHandler m_redrawRequested;
}