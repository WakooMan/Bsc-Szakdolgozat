namespace SevenWonders.GameEngine
{
    public class Engine: IEngine
    {
        private readonly List<IComponent> m_components;

        public ISceneManager SceneManager { get; private set; }
        public IInputManager InputManager { get; private set; }
        public IObjectManager ObjectManager { get; private set; }
        public ISceneFileHandler SceneFileHandler { get; private set; }

        public Engine(ISceneManager sceneManager, IInputManager inputManager, IObjectManager objectManager, ISceneFileHandler sceneFileHandler)
        {
            m_components = new List<IComponent>();
            SceneManager = sceneManager;
            InputManager = inputManager;
            ObjectManager = objectManager;
            SceneFileHandler = sceneFileHandler;
            SceneManager.SceneRegistered += SceneRegistered;
            SceneManager.SceneRemoved += SceneRemoved;
        }

        public void Shutdown()
        {
            m_components.ForEach(component => component.Shutdown());
        }
        public void MainLoop()
        {
            while (true)
            {
                m_components.ForEach(component => component.Update());
            }
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
    }
}
