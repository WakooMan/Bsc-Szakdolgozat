using NSubstitute;
using SevenWonders.Game.Engine;
using SevenWonders.Game.Engine.Components;
using SkiaSharp.Views.Maui.Controls;
using static SevenWonders.Game.Engine.ISceneManager;

namespace SevenWonders.GameEngine_UnitTests
{
    [TestFixture]
    public class EngineTests
    {
        [SetUp]
        public void SetUp()
        {
            m_sceneManager = Substitute.For<ISceneManager>();
            m_inputManager = Substitute.For<IInputManager>();
            m_objectManager = Substitute.For<IObjectManager>();
            m_sceneLoader = Substitute.For<ISceneLoader>();
            m_gameEngineTicker = Substitute.For<IGameEngineTicker>();

            m_engine = new Engine(
                m_sceneManager,
                m_inputManager,
                m_objectManager,
                m_sceneLoader,
                m_gameEngineTicker);
        }

        [Test]
        public void Startup_ShouldStartTimerAndNotifyComponents()
        {
            // Arrange
            var component = Substitute.For<IComponent>();
            m_engine.RegisterSubSystem(component);

            // Act
            m_engine.Startup();

            // Assert
            component.Received(1).Startup();
            m_gameEngineTicker.Received(1).Start();
            Assert.That(m_engine.Configuration.TargetFrameTime, Is.GreaterThan(0));
        }

        [Test]
        public void Shutdown_ShouldStopTimerAndNotifyComponents()
        {
            // Arrange
            var component = Substitute.For<IComponent>();
            m_engine.RegisterSubSystem(component);
            m_engine.Startup();

            // Act
            m_engine.Shutdown();

            // Assert
            m_gameEngineTicker.Received(1).Stop();
            component.Received(1).Shutdown();
        }

        [Test]
        public void SceneRegistered_ShouldSubscribeObjectsToTouchEvents()
        {
            // Arrange
            var gameObject = new GameObject { Id = 1 };
            var layer = new GraphicsLayer { SceneObjectsProxy = new List<SceneObject> { gameObject } };
            var scene = new Scene { Layers = new List<GraphicsLayer> { layer } };

            // Act
            m_sceneManager.SceneRegistered += Raise.Event<SceneEvent>(scene);

            // Assert
            m_objectManager.Received(1).SubscribeInteractiveObjectToTouchEvents(gameObject, layer);
        }

        [Test]
        public void SceneRemoved_ShouldUnsubscribeObjects()
        {
            // Arrange
            var gameObject = new GameObject { Id = 1 };
            var layer = new GraphicsLayer { SceneObjectsProxy = new List<SceneObject> { gameObject } };
            var scene = new Scene { Layers = new List<GraphicsLayer> { layer } };

            // Act
            m_sceneManager.SceneRemoved += Raise.Event<SceneEvent>(scene);

            // Assert
            m_objectManager.Received(1).UnsubscribeInteractiveObjectToTouchEvents(gameObject);
        }

        private ISceneManager m_sceneManager;
        private IInputManager m_inputManager;
        private IObjectManager m_objectManager;
        private ISceneLoader m_sceneLoader;
        private IGameEngineTicker m_gameEngineTicker;
        private Engine m_engine;
    }
}