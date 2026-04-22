using SevenWondersUI.ViewModels;
using SkiaSharp.Views.Maui;
using System.Numerics;

namespace SevenWondersUI.Views;

public partial class PlayerVSAIGamePage : ContentPage
{
    public PlayerVSAIGamePage(PlayerVSAIGamePageViewModel gamePageViewModel)
    {
        m_playerVSAIGamePageViewModel = gamePageViewModel;
        InitializeComponent();
        m_playerVSAIGamePageViewModel.Engine.RedrawRequested += (e, args) =>
        {
            if (m_gameView is not null)
            {
                m_gameView.InvalidateSurface();
            }
        };
        BindingContext = m_playerVSAIGamePageViewModel;
    }
    protected override async void OnAppearing()
    {
        base.OnAppearing();
        while (m_gameView is null)
        {
            await Task.Delay(100);
        }
        await m_playerVSAIGamePageViewModel.Initialize();
        OnCanvasSizeChanged(this, EventArgs.Empty);
    }

    private void OnCanvasSizeChanged(object sender, EventArgs e)
    {
        if (m_playerVSAIGamePageViewModel.Engine.SceneManager.CurrentScene is not null)
        {
            m_playerVSAIGamePageViewModel.Engine.SceneManager.CurrentScene.Resize(new Vector2((float)m_mainGrid.Width, (float)m_mainGrid.Height));
        }
    }


    private void OnPaintSurface(object sender, SKPaintSurfaceEventArgs e)
    {
        m_playerVSAIGamePageViewModel.Engine.SceneManager.Render(e.Surface.Canvas);
    }

    private void OnTouch(object sender, SKTouchEventArgs e)
    {
        m_playerVSAIGamePageViewModel.Engine.InputManager.OnTouchEvent(e);
    }

    private readonly PlayerVSAIGamePageViewModel m_playerVSAIGamePageViewModel;
}