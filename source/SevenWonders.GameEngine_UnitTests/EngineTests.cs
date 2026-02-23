using Microsoft.Maui.Dispatching;
using NSubstitute;
using NUnit.Framework;
using SevenWonders.Common;
using SevenWonders.GameEngine;
using SevenWonders.GameEngine.Components;
using SkiaSharp.Views.Maui.Controls;
using System.Diagnostics;
using System.Runtime.Serialization;
using static SevenWonders.GameEngine.ISceneManager;

namespace SevenWonders.GameEngine_UnitTests
{
    [TestFixture]
    public class EngineTests
    {
        private ISceneManager _sceneManager;
        private IInputManager _inputManager;
        private IObjectManager _objectManager;
        private ISceneLoader _sceneLoader;
        private IDispatcherTimer _dispatcherTimer;
        private SKCanvasView _canvasView;
        private Engine _engine;

        [SetUp]
        public void SetUp()
        {
            _sceneManager = Substitute.For<ISceneManager>();
            _inputManager = Substitute.For<IInputManager>();
            _objectManager = Substitute.For<IObjectManager>();
            _sceneLoader = Substitute.For<ISceneLoader>();
            _dispatcherTimer = Substitute.For<IDispatcherTimer>();
            _canvasView = new SKCanvasView(); // SkiaSharp view-t nehéz mockolni, de példányosítható

            _engine = new Engine(
                _sceneManager,
                _inputManager,
                _objectManager,
                _sceneLoader,
                Substitute.For<IGameEngineTicker>());
        }

        [Test]
        public void Startup_ShouldStartTimerAndNotifyComponents()
        {
            // Arrange
            var component = Substitute.For<IComponent>();
            _engine.RegisterSubSystem(component);

            // Act
            _engine.Startup();

            // Assert
            component.Received(1).Startup();
            _dispatcherTimer.Received(1).Start();
            Assert.That(_engine.Configuration.TargetFrameTime, Is.GreaterThan(0));
        }

        [Test]
        public void Shutdown_ShouldStopTimerAndNotifyComponents()
        {
            // Arrange
            var component = Substitute.For<IComponent>();
            _engine.RegisterSubSystem(component);
            _engine.Startup();

            // Act
            _engine.Shutdown();

            // Assert
            _dispatcherTimer.Received(1).Stop();
            component.Received(1).Shutdown();
        }

        [Test]
        public void SceneRegistered_ShouldSubscribeObjectsToTouchEvents()
        {
            // Arrange
            var gameObject = new GameObject { Id = 1 };
            var layer = new GraphicsLayer { ObjectList = new List<GameObject> { gameObject } };
            var scene = new Scene { Layers = new List<GraphicsLayer> { layer } };

            // Act
            _sceneManager.SceneRegistered += Raise.Event<SceneEvent>(scene);

            // Assert
            _objectManager.Received(1).SubscribeGameObjectToTouchEvents(gameObject, layer);
        }

        [Test]
        public void SceneRemoved_ShouldUnsubscribeObjects()
        {
            // Arrange
            var gameObject = new GameObject { Id = 1 };
            var layer = new GraphicsLayer { ObjectList = new List<GameObject> { gameObject } };
            var scene = new Scene { Layers = new List<GraphicsLayer> { layer } };

            // Act
            _sceneManager.SceneRemoved += Raise.Event<SceneEvent>(scene);

            // Assert
            _objectManager.Received(1).UnsubscribeGameObjectToTouchEvents(gameObject);
        }
    }
}