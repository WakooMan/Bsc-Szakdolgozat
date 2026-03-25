using SevenWonders.GameEngine.Components;
using System.Diagnostics;

namespace SevenWonders.GameEngine
{
    public class Engine: IEngine
    {
        private readonly List<IComponent> m_components;

        public event EventHandler? RedrawRequested;

        public ISceneManager SceneManager { get; private set; }
        public IInputManager InputManager { get; private set; }
        public IObjectManager ObjectManager { get; private set; }
        public ISceneLoader SceneFileHandler { get; private set; }

        public GameEngineConfiguration Configuration { get; }

        public Engine(ISceneManager sceneManager, IInputManager inputManager, IObjectManager objectManager, ISceneLoader sceneFileHandler, IGameEngineTicker gameEngineTicker)
        {
            m_components = new List<IComponent>();
            SceneManager = sceneManager;
            InputManager = inputManager;
            ObjectManager = objectManager;
            SceneFileHandler = sceneFileHandler;
            m_gameEngineTicker = gameEngineTicker;
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

            m_gameEngineTicker.Interval = TimeSpan.FromMilliseconds(Configuration.TargetFrameTime);
            m_gameEngineTicker.Tick += (s, e) =>
            {
                Update();
            };
            m_stopwatch.Start();
            m_gameEngineTicker.Start();
            m_running = true;
        }

        public void Shutdown()
        {
            m_gameEngineTicker.Stop();
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
                layer.Buttons.ForEach(button =>
                {
                    ObjectManager.SubscribeButtonToTouchEvents(button, layer);
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
                RedrawRequested?.Invoke(this, EventArgs.Empty);
            }
        }

        private bool m_running;
        private double m_renderTimer;
        private readonly IGameEngineTicker m_gameEngineTicker;
        private readonly Stopwatch m_stopwatch;
    }
}
