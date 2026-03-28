using NSubstitute;
using SevenWonders.GameEngine;
using SkiaSharp.Views.Maui;

namespace SevenWonders.GameEngine_UnitTests
{
    [TestFixture]
    public class ObjectManagerTests
    {
        private IInputManager _inputManager;
        private ISceneLoader _sceneLoader;
        private ObjectManager _objectManager;
        private Scene _testScene;
        private GraphicsLayer _testLayer;

        [SetUp]
        public void SetUp()
        {
            // Creating substitutes
            _inputManager = Substitute.For<IInputManager>();
            _sceneLoader = Substitute.For<ISceneLoader>();

            _objectManager = new ObjectManager(_inputManager, _sceneLoader);

            // Setting up data
            _testLayer = new GraphicsLayer
            {
                Id = 10,
                Name = "TestLayer",
                ObjectList = new List<GameObject>()
            };

            _testScene = new Scene
            {
                Id = Guid.NewGuid(),
                Layers = new List<GraphicsLayer> { _testLayer },
                BiggestId = 100
            };

            // Mocking a return value
            _sceneLoader.ReceiveSceneFolder(_testScene).Returns("Content/Scenes/Test");
        }

        [Test]
        public void AddGameObject_WhenLayerNotInScene_ShouldThrowException()
        {
            // Arrange
            var foreignLayer = new GraphicsLayer { Name = "Not In Scene" };
            var gameObject = new GameObject();

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() =>
                _objectManager.AddGameObject(_testScene, foreignLayer, gameObject));
        }

        [Test]
        public void AddGameObject_ShouldAssignIdAndLoadTextures()
        {
            // Arrange
            var gameObject = new GameObject { Name = "TestObject" };

            // Act
            _objectManager.AddGameObject(_testScene, _testLayer, gameObject);

            // Assert
            Assert.That(gameObject.Id, Is.EqualTo(100));
            Assert.That(_testScene.BiggestId, Is.EqualTo(101));
            _sceneLoader.Received(1).ReceiveSceneFolder(_testScene);
            Assert.That(_testLayer.ObjectList.Contains(gameObject), Is.True);
        }

        [Test]
        public void AddGameObject_ShouldSubscribeToAllTouchEvents()
        {
            // Arrange
            var gameObject = new GameObject { Name = "InputTarget" };

            // Act
            _objectManager.AddGameObject(_testScene, _testLayer, gameObject);

            // Assert
            // Check that SubscribeTouchEvent was called for each type
            _inputManager.Received().SubscribeTouchEvent(TouchEvent.Released, SKMouseButton.Left, Arg.Any<Action<SKTouchEventArgs>>());
            _inputManager.Received().SubscribeTouchEvent(TouchEvent.Pressed, SKMouseButton.Left, Arg.Any<Action<SKTouchEventArgs>>());
            _inputManager.Received().SubscribeTouchEvent(TouchEvent.Moved, SKMouseButton.Left, Arg.Any<Action<SKTouchEventArgs>>());
            _inputManager.Received().SubscribeTouchEvent(TouchEvent.Clicked, SKMouseButton.Left, Arg.Any<Action<SKTouchEventArgs>>());
        }

        [Test]
        public void RemoveGameObject_WhenObjectExists_ShouldUnsubscribeAndRemove()
        {
            // Arrange
            var gameObject = new GameObject { Id = 1, Name = "ToRemove" };
            _objectManager.AddGameObject(_testScene, _testLayer, gameObject);

            // Act
            _objectManager.RemoveGameObject(_testLayer, gameObject);

            // Assert
            Assert.That(_testLayer.ObjectList.Contains(gameObject), Is.False);

            // Verify unsubscription
            _inputManager.Received().UnsubscribeTouchEvent(TouchEvent.Released, SKMouseButton.Left, Arg.Any<Action<SKTouchEventArgs>>());
            _inputManager.Received().UnsubscribeTouchEvent(TouchEvent.Pressed, SKMouseButton.Left, Arg.Any<Action<SKTouchEventArgs>>());
        }

        //[Test]
        //public void AddTexture_ShouldLoadAndAddToList()
        //{
        //    // Arrange
        //    var texture = new TextureObject { Name = "Grass" };

        //    // Act
        //    _objectManager.AddTexture(_testScene, _testLayer, texture);

        //    // Assert
        //    Assert.That(_testLayer.Textures.Contains(texture), Is.True);
        //    _sceneLoader.Received().ReceiveSceneFolder(_testScene);
        //}

        [Test]
        public void CopyGraphicsLayer_ShouldCreateDeepCopyAndAddtoScene()
        {
            // Arrange
            string newName = "CopiedLayer";

            // Act
            var result = _objectManager.CopyGraphicsLayer(_testScene, _testLayer, newName);

            // Assert
            Assert.That(newName, Is.EqualTo(result.Name));
            Assert.That(_testScene.Layers.Contains(result), Is.True);
            // Verify that the ID was incremented for the copy
            Assert.That(_testLayer.Id, Is.Not.EqualTo(result.Id));
        }
    }
}