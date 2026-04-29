using SevenWondersUI.ViewModels;
using SkiaSharp.Views.Maui;
using System.Numerics;

namespace SevenWondersUI.Views;

public partial class PlayerVSPlayerGamePage : ContentPage
{
    public PlayerVSPlayerGamePage(PlayerVSPlayerGamePageViewModel playerVSPlayerGamePageViewModel)
    {
        m_playerVSPlayerGamePageViewModel = playerVSPlayerGamePageViewModel;
        InitializeComponent();
        m_redrawRequested = (e, args) =>
        {
            if (m_gameView is not null)
            {
                m_gameView.InvalidateSurface();
            }
        };
        BindingContext = m_playerVSPlayerGamePageViewModel;
    }
    protected override async void OnAppearing()
    {
        base.OnAppearing();
        while (m_gameView is null)
        {
            await Task.Delay(100);
        }
        m_playerVSPlayerGamePageViewModel.GameHandler.SubscribeRedrawRequested(m_redrawRequested);
        await m_playerVSPlayerGamePageViewModel.Initialize();
        OnCanvasSizeChanged(this, EventArgs.Empty);
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        m_playerVSPlayerGamePageViewModel.GameHandler.UnsubscribeRedrawRequested(m_redrawRequested);
    }

    private void OnCanvasSizeChanged(object sender, EventArgs e)
    {
        m_playerVSPlayerGamePageViewModel.GameHandler.Resize(new Vector2((float)m_mainGrid.Width, (float)m_mainGrid.Height));
    }


    private void OnPaintSurface(object sender, SKPaintSurfaceEventArgs e)
    {
        m_playerVSPlayerGamePageViewModel.GameHandler.Render(e.Surface.Canvas);
    }

    private void OnTouch(object sender, SKTouchEventArgs e)
    {
        m_playerVSPlayerGamePageViewModel.GameHandler.OnTouchEvent(e);
    }

    private readonly PlayerVSPlayerGamePageViewModel m_playerVSPlayerGamePageViewModel;
    private readonly EventHandler m_redrawRequested;
}