using GameLogic;
using GameLogic.Elements.Wonders;
using GameLogic.Events.GameEvents;
using SevenWonders.Common;
using SevenWonders.GameEngine;
using SevenWonders.GameEngine.Components;
using SevenWonders.Presenter.Presenters;
using SkiaSharp.Views.Maui;
using System.Numerics;

namespace SevenWondersUI
{
    public partial class MainPage : ContentPage
    {
        public MainPage(IGame game, IEngine engine, ISceneLoader sceneLoader, IMoverComponent moverComponent, ICardFlipComponent cardFlipComponent, IWonderPresenter wonderPresenter)
        {
            m_wonderPresenter = wonderPresenter;
            m_moverComponent = moverComponent;
            m_cardFlipComponent = cardFlipComponent;
            m_sceneLoader = sceneLoader;
            m_game = game;
            m_engine = engine;
            m_engine.RedrawRequested += (e, args) => canvas?.InvalidateSurface();
        }
        protected override async void OnAppearing()
        {
            base.OnAppearing();

            InitializeComponent();
            GameLog.InitializeFileLogger();
            m_engine.RegisterSubSystem(m_moverComponent);
            m_engine.RegisterSubSystem(m_cardFlipComponent);

            foreach (Scene scene in await m_sceneLoader.LoadScenes())
            {
                m_engine.SceneManager.RegisterScene(scene);
            }

            Scene? firstScene = m_engine.SceneManager.Scenes.FirstOrDefault();

            if (firstScene != null)
            {
                m_engine.SceneManager.SetCurrentScene(firstScene);
            }

            m_wonderPresenter.Initialize();
            m_game.Initialize("Player1", "Player2");
            m_game.Context.EventManager.Subscribe<OnChooseWonderStateStart>(state => {
                foreach (Wonder wonder in m_game.Context.ChooseWonderHandler.FirstChoosableWonders)
                {
                    m_wonderPresenter.MoveToCenter(wonder);
                }
            });
            m_engine.Startup();
            m_game.Context.EventManager.Publish(new OnChooseWonderStateStart());
        }

        private void OnCanvasSizeChanged(object sender, EventArgs e)
        {
            Grid? grid = sender as Grid;
            if (grid != null)
            {
                m_width = (float)grid.Width;
                m_height = (float)grid.Height;
                if (m_engine.SceneManager.CurrentScene is not null)
                {
                    m_engine.SceneManager.CurrentScene.Resize(new Vector2(m_width, m_height));
                }
            }
        }


        private void OnCanvasViewPaintSurface(object sender, SKPaintSurfaceEventArgs e)
        {
            e.Surface.Canvas.Clear();
            if (m_engine.SceneManager.CurrentScene is not null)
            {
                m_engine.SceneManager.CurrentScene.Draw(e);
            }
        }

        private void OnTouchEffectAction(object sender, SKTouchEventArgs e)
        {
            m_engine.InputManager.OnTouchEvent(e);
        }

        private float m_width = 1600;
        private float m_height = 900;
        private readonly ISceneLoader m_sceneLoader;
        private readonly IEngine m_engine;
        private readonly IGame m_game;
        private readonly ICardFlipComponent m_cardFlipComponent;
        private readonly IMoverComponent m_moverComponent;
        private readonly IWonderPresenter m_wonderPresenter;

    }

}
