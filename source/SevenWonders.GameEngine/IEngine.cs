using SkiaSharp.Views.Maui.Controls;

namespace SevenWonders.GameEngine
{
    public interface IEngine
    {
        ISceneManager SceneManager { get; }
        IInputManager InputManager { get; }
        IObjectManager ObjectManager { get; }
        ISceneLoader SceneFileHandler { get; }
        GameEngineConfiguration Configuration { get; }

        void Startup();
        void Shutdown();
        void RegisterSubSystem(IComponent component);
    }
}