using NUnit.Framework;
using NSubstitute;
using Microsoft.Maui.Dispatching;
using SkiaSharp.Views.Maui.Controls;
using System.Diagnostics;
using SevenWonders.GameEngine;
using SevenWonders.Common;

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
                _dispatcherTimer,
                _canvasView);
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

        //[Test]
        //public void SceneRegistered_ShouldSubscribeObjectsToTouchEvents()
        //{
        //    // Arrange
        //    var gameObject = new GameObject { Id = 1 };
        //    var layer = new GraphicsLayer { ObjectList = new List<GameObject> { gameObject } };
        //    var scene = new Scene { Layers = new List<GraphicsLayer> { layer } };

        //    // Act - Szimuláljuk az esemény kiváltását a SceneManageren keresztül
        //    _sceneManager.SceneRegistered += Raise.Event<Action<Scene>>(scene);

        //    // Assert
        //    _objectManager.Received(1).SubscribeGameObjectToTouchEvents(gameObject, layer);
        //}

        //[Test]
        //public void SceneRemoved_ShouldUnsubscribeObjects()
        //{
        //    // Arrange
        //    var gameObject = new GameObject { Id = 1 };
        //    var layer = new GraphicsLayer { ObjectList = new List<GameObject> { gameObject } };
        //    var scene = new Scene { Layers = new List<GraphicsLayer> { layer } };

        //    // Act
        //    _sceneManager.SceneRemoved += Raise.Event<Action<Scene>>(scene);

        //    // Assert
        //    _objectManager.Received(1).UnsubscribeGameObjectToTouchEvents(gameObject);
        //}
    }
}