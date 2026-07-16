using SevenWonders.Game.Engine.Components;
using SevenWonders.Game.Engine.InputHandling;
using SevenWonders.Game.Engine.SceneHandling;
using SkiaSharp.Views.Maui.Controls;

namespace SevenWonders.Game.Engine
{
    public interface IEngine
    {
        event EventHandler? RedrawRequested;
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