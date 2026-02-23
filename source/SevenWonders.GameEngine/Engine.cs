using Microsoft.Maui.Dispatching;
using SkiaSharp.Views.Maui;
using SkiaSharp.Views.Maui.Controls;
using System.Diagnostics;

namespace SevenWonders.GameEngine
{
    public class Engine: IEngine
    {
        private readonly List<IComponent> m_components;

        public ISceneManager SceneManager { get; private set; }
        public IInputManager InputManager { get; private set; }
        public IObjectManager ObjectManager { get; private set; }
        public ISceneLoader SceneFileHandler { get; private set; }

        public GameEngineConfiguration Configuration { get; }

        public Engine(ISceneManager sceneManager, IInputManager inputManager, IObjectManager objectManager, ISceneLoader sceneFileHandler, IDispatcherTimer dispatcherTimer, SKCanvasView canvasView)
        {
            m_components = new List<IComponent>();
            SceneManager = sceneManager;
            InputManager = inputManager;
            ObjectManager = objectManager;
            SceneFileHandler = sceneFileHandler;
            m_dispatcherTimer = dispatcherTimer;
            m_canvasView = canvasView;
            SceneManager.SceneRegistered += SceneRegistered;
            SceneManager.SceneRemoved += SceneRemoved;
            Configuration = new GameEngineConfiguration(60);
            m_renderTimer = 0;
            m_stopwatch = new Stopwatch();
            m_running = false;
        }

        public void Startup()
        {
            m_components.ForEach(component => component.Startup());
            m_renderTimer = 0;

            m_dispatcherTimer.Interval = TimeSpan.FromMilliseconds(Configuration.TargetFrameTime);
            m_dispatcherTimer.Tick += (s, e) =>
            {
                Update();
            };
            m_stopwatch.Start();
            m_dispatcherTimer.Start();
            m_running = true;
        }

        public void Shutdown()
        {
            m_dispatcherTimer.Stop();
            m_stopwatch.Stop();
            m_running = false;
            m_renderTimer = 0;
            m_components.ForEach(component => component.Shutdown());
        }

        public void RegisterSubSystem(IComponent component)
        {
            m_components.Add(component);
        }

        private void SceneRegistered(Scene scene)
        {
            scene.Layers.ForEach(layer =>
            {
                layer.ObjectList.ForEach(obj =>
                {
                    ObjectManager.SubscribeGameObjectToTouchEvents(obj, layer);
                });
            });
        }

        private void SceneRemoved(Scene scene)
        {
            scene.Layers.ForEach(layer =>
            {
                layer.ObjectList.ForEach(obj =>
                {
                    ObjectManager.UnsubscribeGameObjectToTouchEvents(obj);
                });
            });
        }

        private void Update()
        {
            if (m_running)
            {
                double currentTimestamp = m_stopwatch.Elapsed.TotalSeconds;
                double deltaTime = (currentTimestamp - m_renderTimer);
                m_renderTimer = currentTimestamp;

                m_components.ForEach(c => c.Update((float)deltaTime));
                m_canvasView.InvalidateSurface();
            }
        }

        private bool m_running;
        private double m_renderTimer;
        private readonly IDispatcherTimer m_dispatcherTimer;
        private readonly SKCanvasView m_canvasView;
        private readonly Stopwatch m_stopwatch;
    }
}
