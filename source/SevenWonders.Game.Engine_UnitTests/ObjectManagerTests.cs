using NSubstitute;
using SevenWonders.Game.Engine;
using SkiaSharp.Views.Maui;
using System.Linq;

namespace SevenWonders.GameEngine_UnitTests
{
    [TestFixture]
    public class ObjectManagerTests
    {

        [SetUp]
        public void SetUp()
        {
            m_inputManager = Substitute.For<IInputManager>();
            m_sceneLoader = Substitute.For<ISceneLoader>();
            m_sceneManager = Substitute.For<ISceneManager>();

            m_objectManager = new ObjectManager(m_inputManager, m_sceneLoader, m_sceneManager);

            m_testLayer = new GraphicsLayer
            {
                Id = 10,
                Name = "TestLayer",
                SceneObjectsProxy = new List<SceneObject>()
            };

            m_testScene = new Scene
            {
                Id = Guid.NewGuid(),
                Layers = new List<GraphicsLayer> { m_testLayer }
            };

            m_sceneLoader.ReceiveSceneFolder(m_testScene).Returns("Content/Scenes/Test");
        }

        [TearDown]
        public void TearDown()
        {
            m_testScene?.Dispose();
        }

        [Test]
        public void AddGameObject_WhenLayerNotInScene_ShouldThrowException()
        {
            // Arrange
            var foreignLayer = new GraphicsLayer { Name = "Not In Scene" };
            var gameObject = new GameObject();

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() =>
                m_objectManager.AddSceneObject(m_testScene, foreignLayer, gameObject));
        }

        [Test]
        public void AddGameObject_ShouldAssignIdAndLoadTextures()
        {
            // Arrange
            var gameObject = new GameObject { Name = "TestObject" };

            // Act
            m_objectManager.AddSceneObject(m_testScene, m_testLayer, gameObject);

            // Assert
            Assert.That(gameObject.Id, Is.EqualTo(0));
            Assert.That(m_testLayer.SceneObjectsProxy.Contains(gameObject), Is.True);
        }

        [Test]
        public void AddGameObject_Without_Scene_ShouldAssignIdAndLoadTextures()
        {
            // Arrange
            var gameObject = new GameObject { Name = "TestObject" };
            m_sceneManager.CurrentScene.Returns(m_testScene);

            // Act
            m_objectManager.AddSceneObject(m_testLayer, gameObject);

            // Assert
            Assert.That(gameObject.Id, Is.EqualTo(0));
            Assert.That(m_testLayer.SceneObjectsProxy.Contains(gameObject), Is.True);
        }

        [Test]
        public void AddGameObject_ShouldSubscribeToAllTouchEvents()
        {
            // Arrange
            var gameObject = new GameObject { Name = "InputTarget" };

            // Act
            m_objectManager.AddInteractiveObject(m_testScene, m_testLayer, gameObject);

            // Assert
            m_inputManager.Received().SubscribeTouchEvent(TouchEvent.Released, SKMouseButton.Left, Arg.Any<Action<SKTouchEventArgs>>());
            m_inputManager.Received().SubscribeTouchEvent(TouchEvent.Pressed, SKMouseButton.Left, Arg.Any<Action<SKTouchEventArgs>>());
            m_inputManager.Received().SubscribeTouchEvent(TouchEvent.Moved, SKMouseButton.Left, Arg.Any<Action<SKTouchEventArgs>>());
            m_inputManager.Received().SubscribeTouchEvent(TouchEvent.Clicked, SKMouseButton.Left, Arg.Any<Action<SKTouchEventArgs>>());
        }

        [Test]
        public void RemoveGameObject_WhenObjectExists_ShouldUnsubscribeAndRemove()
        {
            // Arrange
            var gameObject = new GameObject { Id = 1, Name = "ToRemove" };
            m_objectManager.AddInteractiveObject(m_testScene, m_testLayer, gameObject);

            // Act
            m_objectManager.RemoveInteractiveObject(m_testLayer, gameObject);

            // Assert
            Assert.That(m_testLayer.SceneObjectsProxy.Contains(gameObject), Is.False);
            m_inputManager.Received().UnsubscribeTouchEvent(TouchEvent.Released, SKMouseButton.Left, Arg.Any<Action<SKTouchEventArgs>>());
            m_inputManager.Received().UnsubscribeTouchEvent(TouchEvent.Pressed, SKMouseButton.Left, Arg.Any<Action<SKTouchEventArgs>>());
        }

        [Test]
        public void CopyGraphicsLayer_ShouldCreateDeepCopyAndAddtoScene()
        {
            // Arrange
            string newName = "CopiedLayer";

            // Act
            var result = m_objectManager.CopyGraphicsLayer(m_testScene, m_testLayer, newName);

            // Assert
            Assert.That(newName, Is.EqualTo(result.Name));
            Assert.That(m_testScene.Layers.Contains(result), Is.True);
            Assert.That(m_testLayer.Id, Is.Not.EqualTo(result.Id));
        }

        [Test]
        public void CopyGameObject_ShouldCreateDeepCopyAndAddtoLayer()
        {
            // Arrange
            var gameObject = new GameObject { Name = "InputTarget", Id= 100 };

            // Act
            GameObject result = m_objectManager.CopyGameObject(m_testScene, m_testLayer, gameObject, "Copy");

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(m_testLayer.SceneObjects.Contains(gameObject), Is.False);
            Assert.That(m_testLayer.SceneObjects.Contains(result), Is.True);
            Assert.That(result == gameObject, Is.False);
            Assert.That(result.Id == gameObject.Id, Is.False);
            Assert.That(ReferenceEquals(result, gameObject), Is.False);
        }

        [Test]
        public void CopyGameObject_Without_Scene_ShouldCreateDeepCopyAndAddtoLayer()
        {
            // Arrange
            var gameObject = new GameObject { Name = "InputTarget", Id= 100 };
            m_sceneManager.CurrentScene.Returns(m_testScene);

            // Act
            GameObject result = m_objectManager.CopyGameObject(m_testLayer, gameObject, "Copy");

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(m_testLayer.SceneObjects.Contains(gameObject), Is.False);
            Assert.That(m_testLayer.SceneObjects.Contains(result), Is.True);
            Assert.That(result == gameObject, Is.False);
            Assert.That(result.Id == gameObject.Id, Is.False);
            Assert.That(ReferenceEquals(result, gameObject), Is.False);
        }

        [Test]
        public void CopyButtonObject_ShouldCreateDeepCopyAndAddtoLayer()
        {
            // Arrange
            var buttonObject = new ButtonObject { Name = "InputTarget", Id= 100 };

            // Act
            ButtonObject result = m_objectManager.CopyButtonObject(m_testScene, m_testLayer, buttonObject, "Copy");

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(m_testLayer.SceneObjects.Contains(buttonObject), Is.False);
            Assert.That(m_testLayer.SceneObjects.Contains(result), Is.True);
            Assert.That(result == buttonObject, Is.False);
            Assert.That(result.Id == buttonObject.Id, Is.False);
            Assert.That(ReferenceEquals(result, buttonObject), Is.False);
        }

        [Test]
        public void CopyTextLabel_ShouldCreateDeepCopyAndAddtoLayer()
        {
            // Arrange
            var textLabel = new TextLabel { Name = "InputTarget", Id= 100 };

            // Act
            TextLabel result = m_objectManager.CopyTextLabel(m_testScene, m_testLayer, textLabel, "Copy");

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(m_testLayer.SceneObjects.Contains(textLabel), Is.False);
            Assert.That(m_testLayer.SceneObjects.Contains(result), Is.True);
            Assert.That(result == textLabel, Is.False);
            Assert.That(result.Id == textLabel.Id, Is.False);
            Assert.That(ReferenceEquals(result, textLabel), Is.False);
        }


        [Test]
        public void CopyTextureObject_ShouldCreateDeepCopyAndAddtoLayer()
        {
            // Arrange
            var textureObject = new TextureObject { Name = "InputTarget", Id= 100 };

            // Act
            TextureObject result = m_objectManager.CopyTextureObject(m_testScene, m_testLayer, textureObject, "Copy");

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(m_testLayer.SceneObjects.Contains(textureObject), Is.False);
            Assert.That(m_testLayer.SceneObjects.Contains(result), Is.True);
            Assert.That(result == textureObject, Is.False);
            Assert.That(result.Id == textureObject.Id, Is.False);
            Assert.That(ReferenceEquals(result, textureObject), Is.False);
        }

        private ISceneManager m_sceneManager;
        private IInputManager m_inputManager;
        private ISceneLoader m_sceneLoader;
        private ObjectManager m_objectManager;
        private Scene m_testScene;
        private GraphicsLayer m_testLayer;
    }
}