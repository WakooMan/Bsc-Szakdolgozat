using SevenWonders.UI.ViewModels;
using SkiaSharp.Views.Maui;
using System.Numerics;

namespace SevenWonders.UI.Views;

public partial class PlayerVSAIGamePage : ContentPage
{
    public PlayerVSAIGamePage(PlayerVSAIGamePageViewModel gamePageViewModel)
    {
        m_playerVSAIGamePageViewModel = gamePageViewModel;
        m_resizeNeeded = false;
        m_redrawRequested = (e, args) =>
        {
            if (m_gameView is not null)
            {
                m_gameView.InvalidateSurface();
            }
        };
        InitializeComponent();
        BindingContext = m_playerVSAIGamePageViewModel;
    }
    protected override async void OnAppearing()
    {
        base.OnAppearing();
        while (m_gameView is null)
        {
            await Task.Delay(100);
        }

        m_playerVSAIGamePageViewModel.GameHandler.SubscribeRedrawRequested(m_redrawRequested);
        await m_playerVSAIGamePageViewModel.Initialize();
        OnCanvasSizeChanged(this, EventArgs.Empty);
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        m_playerVSAIGamePageViewModel.GameHandler.UnsubscribeRedrawRequested(m_redrawRequested);
    }

    private void OnCanvasSizeChanged(object sender, EventArgs e)
    {
        m_resizeNeeded = true;
    }


    private void OnPaintSurface(object sender, SKPaintSurfaceEventArgs e)
    {

        if (m_resizeNeeded)
        {
            m_playerVSAIGamePageViewModel.GameHandler.Resize(new Vector2(e.Info.Width, e.Info.Height));
            m_resizeNeeded = false;
        }

        m_playerVSAIGamePageViewModel.GameHandler.Render(e.Surface.Canvas);
    }

    private void OnTouch(object sender, SKTouchEventArgs e)
    {
        m_playerVSAIGamePageViewModel.GameHandler.OnTouchEvent(e);
        e.Handled = true;
    }

    private readonly PlayerVSAIGamePageViewModel m_playerVSAIGamePageViewModel;
    private readonly EventHandler m_redrawRequested;
    private bool m_resizeNeeded;
}