using SevenWonders.WebClient.Model;
using SevenWondersUI.ViewModels;
using SkiaSharp.Views.Maui;
using System.Numerics;

namespace SevenWondersUI.Views;

public partial class GamePage : ContentPage
{
    public GamePage(GamePageViewModel gamePageViewModel, IClientMessageDispatcher clientMessageDispatcher)
    {
        m_gamePageViewModel = gamePageViewModel;
        m_clientMessageDispatcher = clientMessageDispatcher;
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
        while (m_gameView is null)
        {
            await Task.Delay(100);
        }
        m_clientMessageDispatcher.RegisterHandler(m_gamePageViewModel);
        await m_gamePageViewModel.Initialize();
        OnCanvasSizeChanged(this, EventArgs.Empty);
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        m_clientMessageDispatcher.UnregisterHandler(m_gamePageViewModel);
    }

    private void OnCanvasSizeChanged(object sender, EventArgs e)
    {
        if (m_gamePageViewModel.Engine.SceneManager.CurrentScene is not null)
        {
            m_gamePageViewModel.Engine.SceneManager.CurrentScene.Resize(new Vector2((float)m_mainGrid.Width, (float)m_mainGrid.Height));
        }
    }


    private void OnPaintSurface(object sender, SKPaintSurfaceEventArgs e)
    {
        m_gamePageViewModel.Engine.SceneManager.Render(e.Surface.Canvas);
    }

    private void OnTouch(object sender, SKTouchEventArgs e)
    {
        m_gamePageViewModel.Engine.InputManager.OnTouchEvent(e);
    }

    private readonly GamePageViewModel m_gamePageViewModel;
    private readonly IClientMessageDispatcher m_clientMessageDispatcher;
}