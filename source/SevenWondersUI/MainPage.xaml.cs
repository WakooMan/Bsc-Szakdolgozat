using GameLogic.Elements;
using GameLogic.Elements.Developments;
using GameLogic.Elements.GameCards;
using GameLogic.Elements.Military;
using GameLogic.Elements.Wonders;
using SevenWonders.Common;
using SevenWonders.GameEngine;
using SkiaSharp.Views.Maui;
using System.Numerics;

namespace SevenWondersUI
{
    public partial class MainPage : ContentPage
    {

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            InitializeComponent();
            GameLog.InitializeFileLogger();
            IXmlHandler xmlHandler = new XmlHandler();
            IMilitaryBoard militaryBoard = new MilitaryBoardFactory(xmlHandler).Create();
            IGameElements gameElements = new GameElements(new MainCardListFactory(xmlHandler), new WonderListFactory(xmlHandler), new DevelopmentListFactory(xmlHandler));
            var sm = gameElements.Developments;

            IInputManager inputManager = new InputManager();
            m_sceneLoader = new SceneLoader(new XmlHandler(), new MauiZipFileReceiver());
            IObjectManager objectManager = new ObjectManager(inputManager, m_sceneLoader);
            m_engine = new Engine(new SceneManager(), inputManager, objectManager, m_sceneLoader, Dispatcher.CreateTimer(), canvas);
            MoverComponent moverComponent = new MoverComponent();
            CardFlipComponent cardFlipComponent = new CardFlipComponent();
            m_engine.RegisterSubSystem(moverComponent);
            m_engine.RegisterSubSystem(cardFlipComponent);

            foreach (Scene scene in await m_sceneLoader.LoadScenes())
            {
                m_engine.SceneManager.RegisterScene(scene);
            }

            Scene? firstScene = m_engine.SceneManager.Scenes.FirstOrDefault();

            if (firstScene != null)
            {
                m_engine.SceneManager.SetCurrentScene(firstScene);
            }

            GameObject gameObject =  m_engine.SceneManager.GetObjectByName("Sphinx");
            GameObject wonder1 = m_engine.SceneManager.GetObjectByName("Wonder1");

            m_engine.Startup();
            moverComponent.MoveTo(gameObject, wonder1, 210, 30);
            cardFlipComponent.Flip(gameObject, 0, 0.6f);
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
        private ISceneLoader m_sceneLoader;
        private IEngine m_engine;

    }

}
