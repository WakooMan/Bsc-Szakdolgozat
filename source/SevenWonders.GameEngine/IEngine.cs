namespace SevenWonders.GameEngine
{
    public interface IEngine
    {
        ISceneManager SceneManager { get; }
        void Shutdown();
        void MainLoop();
        void RegisterSubSystem(IComponent component);
    }
}