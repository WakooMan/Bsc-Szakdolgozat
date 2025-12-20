namespace SevenWonders.GameEngine
{
    public interface IEngine
    {
        ISceneManager SceneManager { get; }
        public IInputManager InputManager { get; }
        public IObjectManager ObjectManager { get; }
        public ISceneFileHandler SceneFileHandler { get; }
        void Shutdown();
        void MainLoop();
        void RegisterSubSystem(IComponent component);
    }
}