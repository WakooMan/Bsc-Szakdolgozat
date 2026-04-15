using SevenWondersUI.ViewModels;
using SkiaSharp.Views.Maui;
using System.Numerics;

namespace SevenWondersUI.Views;

public partial class GamePage : ContentPage
{
    public GamePage(GamePageViewModel gamePageViewModel)
    {
        m_gamePageViewModel = gamePageViewModel;
        m_gamePageViewModel.Engine.RedrawRequested += (e, args) => canvas?.InvalidateSurface();
        BindingContext = m_gamePageViewModel;
    }
    protected override async void OnAppearing()
    {
        base.OnAppearing();

        InitializeComponent();
        await m_gamePageViewModel.Initialize();
    }

    private void OnCanvasSizeChanged(object sender, EventArgs e)
    {
        Grid? grid = sender as Grid;
        if (grid != null)
        {
            m_width = (float)grid.Width;
            m_height = (float)grid.Height;
            if (m_gamePageViewModel.Engine.SceneManager.CurrentScene is not null)
            {
                m_gamePageViewModel.Engine.SceneManager.CurrentScene.Resize(new Vector2(m_width, m_height));
            }
        }
    }


    private void OnCanvasViewPaintSurface(object sender, SKPaintSurfaceEventArgs e)
    {
        e.Surface.Canvas.Clear();
        if (m_gamePageViewModel.Engine.SceneManager.CurrentScene is not null)
        {
            m_gamePageViewModel.Engine.SceneManager.CurrentScene.Draw(e.Surface.Canvas);
        }
    }

    private void OnTouchEffectAction(object sender, SKTouchEventArgs e)
    {
        m_gamePageViewModel.Engine.InputManager.OnTouchEvent(e);
    }

    private float m_width = 1600;
    private float m_height = 900;
    private readonly GamePageViewModel m_gamePageViewModel;
}