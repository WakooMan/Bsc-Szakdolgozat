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
        m_playerVSPlayerGamePageViewModel.Engine.RedrawRequested += (e, args) =>
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
        await m_playerVSPlayerGamePageViewModel.Initialize();
        OnCanvasSizeChanged(this, EventArgs.Empty);
    }

    private void OnCanvasSizeChanged(object sender, EventArgs e)
    {
        if (m_playerVSPlayerGamePageViewModel.Engine.SceneManager.CurrentScene is not null)
        {
            m_playerVSPlayerGamePageViewModel.Engine.SceneManager.CurrentScene.Resize(new Vector2((float)m_mainGrid.Width, (float)m_mainGrid.Height));
        }
    }


    private void OnPaintSurface(object sender, SKPaintSurfaceEventArgs e)
    {
        m_playerVSPlayerGamePageViewModel.Engine.SceneManager.Render(e.Surface.Canvas);
    }

    private void OnTouch(object sender, SKTouchEventArgs e)
    {
        m_playerVSPlayerGamePageViewModel.Engine.InputManager.OnTouchEvent(e);
    }

    private readonly PlayerVSPlayerGamePageViewModel m_playerVSPlayerGamePageViewModel;
}