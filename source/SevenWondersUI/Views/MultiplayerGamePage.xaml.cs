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
        m_multiplayerGamePageViewModel.Engine.RedrawRequested += (e, args) =>
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
        await m_multiplayerGamePageViewModel.Initialize();
        OnCanvasSizeChanged(this, EventArgs.Empty);
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        m_clientMessageDispatcher.UnregisterHandler(m_multiplayerGamePageViewModel);
    }

    private void OnCanvasSizeChanged(object sender, EventArgs e)
    {
        if (m_multiplayerGamePageViewModel.Engine.SceneManager.CurrentScene is not null)
        {
            m_multiplayerGamePageViewModel.Engine.SceneManager.CurrentScene.Resize(new Vector2((float)m_mainGrid.Width, (float)m_mainGrid.Height));
        }
    }


    private void OnPaintSurface(object sender, SKPaintSurfaceEventArgs e)
    {
        m_multiplayerGamePageViewModel.Engine.SceneManager.Render(e.Surface.Canvas);
    }

    private void OnTouch(object sender, SKTouchEventArgs e)
    {
        m_multiplayerGamePageViewModel.Engine.InputManager.OnTouchEvent(e);
    }

    private readonly MultiplayerGamePageViewModel m_multiplayerGamePageViewModel;
    private readonly IClientMessageDispatcher m_clientMessageDispatcher;
}