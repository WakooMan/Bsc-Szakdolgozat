using SevenWondersUI.ViewModels;
using SkiaSharp.Views.Maui;
using System.Numerics;

namespace SevenWondersUI.Views;

public partial class GamePage : ContentPage
{
    public GamePage(GamePageViewModel gamePageViewModel)
    {
        m_gamePageViewModel = gamePageViewModel;
        InitializeComponent();
        m_gamePageViewModel.Engine.RedrawRequested += (e, args) =>
        {
            if (m_gameView is not null)
            {
                m_gameView.InvalidateSurface();
            }
        };
        BindingContext = m_gamePageViewModel;
    }
    protected override async void OnAppearing()
    {
        base.OnAppearing();

        await m_gamePageViewModel.Initialize();
        OnCanvasSizeChanged(this, EventArgs.Empty);
    }

    private void OnCanvasSizeChanged(object sender, EventArgs e)
    {
        if (m_gamePageViewModel.Engine.SceneManager.CurrentScene is not null)
        {
            m_gamePageViewModel.Engine.SceneManager.CurrentScene.Resize(new Vector2((float)m_mainGrid.Width, (float)m_mainGrid.Height));
        }
    }


    private void OnPaintSurface(object sender, SKPaintGLSurfaceEventArgs e)
    {
        m_gamePageViewModel.Engine.SceneManager.Render(e.Surface.Canvas);
    }

    private void OnTouch(object sender, SKTouchEventArgs e)
    {
        m_gamePageViewModel.Engine.InputManager.OnTouchEvent(e);
    }

    private readonly GamePageViewModel m_gamePageViewModel;
}