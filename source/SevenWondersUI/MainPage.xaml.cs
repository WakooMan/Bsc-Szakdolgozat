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
        public MainPage(IGame game, IEngine engine, ISceneLoader sceneLoader, IAnimationManager animationManager, IWonderPresenter wonderPresenter, ICardPresenter cardPresenter)
        {
            m_wonderPresenter = wonderPresenter;
            m_cardPresenter = cardPresenter;
            m_animationManager = animationManager;
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
            m_engine.RegisterSubSystem(m_animationManager);

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
            m_cardPresenter.Initialize();
            m_game.Initialize("Player1", "Player2");
            m_game.Context.EventManager.Subscribe<OnChooseWonderStateStart>(state => {
                foreach (Wonder wonder in state.Wonders)
                {
                    m_wonderPresenter.MoveToCenter(wonder);
                }
            });
            m_game.Context.EventManager.Subscribe<OnFourWondersChosen>(state => {
                foreach (Wonder wonder in state.Wonders)
                {
                    m_wonderPresenter.MoveToCenter(wonder);
                }
            });
            m_game.Context.EventManager.Subscribe<OnChooseWonderStateEnd>(state => {
                foreach (Wonder wonder in state.Wonders)
                {
                    m_wonderPresenter.MoveToDeck(wonder);
                }
            });
            m_engine.Startup();
            _ = Task.Run(m_game.GameLoop);

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
        private readonly IAnimationManager m_animationManager;
        private readonly IWonderPresenter m_wonderPresenter;
        private readonly ICardPresenter m_cardPresenter;

    }

}
