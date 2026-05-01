using NUnit.Framework;
using System;
using System.Collections.Generic;
using SevenWonders.Game.Engine;
using System.Linq;
using SkiaSharp.Views.Maui;
using NSubstitute;

namespace SevenWonders.GameEngine_UnitTests
{
    [TestFixture]
    public class SceneManagerTests
    {
        private SceneManager _sceneManager;
        private Scene m_testScene;

        [SetUp]
        public void SetUp()
        {
            _sceneManager = new SceneManager();
            m_testScene = new Scene { Id = Guid.NewGuid(), Name = "MenuScene" };
        }

        [TearDown]
        public void TearDown()
        {
            m_testScene?.Dispose();
        }

        [Test]
        public void RegisterScene_ShouldAddSceneAndFireEvent()
        {
            // Arrange
            bool eventFired = false;
            _sceneManager.SceneRegistered += (scene) => eventFired = true;

            // Act
            _sceneManager.RegisterScene(m_testScene);

            // Assert
            Assert.That(_sceneManager.Scenes.Count, Is.EqualTo(1));
            Assert.That(eventFired, Is.True);
        }

        [Test]
        public void RegisterScene_ShouldNotAddDuplicateScene()
        {
            // Act
            _sceneManager.RegisterScene(m_testScene);
            _sceneManager.RegisterScene(m_testScene);

            // Assert
            Assert.That(_sceneManager.Scenes.Count, Is.EqualTo(1));
        }

        [Test]
        public void GetScene_Success()
        {
            // Act
            _sceneManager.RegisterScene(m_testScene);
            Scene scene = _sceneManager.GetScene(m_testScene.Id);

            // Assert
            Assert.That(scene, Is.EqualTo(m_testScene));
        }

        [Test]
        public void GetScene_Throws_Exception()
        {
            Assert.Throws<InvalidOperationException>(() => _sceneManager.GetScene(m_testScene.Id));
        }

        [Test]
        public void GetSceneByName_Success()
        {
            // Act
            _sceneManager.RegisterScene(m_testScene);
            Scene scene = _sceneManager.GetSceneByName(m_testScene.Name);

            // Assert
            Assert.That(scene, Is.EqualTo(m_testScene));
        }

        [Test]
        public void GetSceneByName_Throws_Exception()
        {
            Assert.Throws<InvalidOperationException>(() => _sceneManager.GetSceneByName(m_testScene.Name));
        }

        [Test]
        public void SetCurrentScene_ShouldWork_IfSceneIsRegistered()
        {
            // Arrange
            _sceneManager.RegisterScene(m_testScene);

            // Act
            _sceneManager.SetCurrentScene(m_testScene);

            // Assert
            Assert.That(m_testScene, Is.EqualTo(_sceneManager.CurrentScene));
        }

        [Test]
        public void SetCurrentScene_ShouldThrowException_IfSceneNotRegistered()
        {
            // Act & Assert
            Assert.Throws<ArgumentException>(() => _sceneManager.SetCurrentScene(m_testScene));
        }

        [Test]
        public void GetObjectByName_ShouldReturnCorrectObject()
        {
            // Arrange
            var targetObj = new GameObject { Name = "FindMe" };
            var layer = new GraphicsLayer { SceneObjectsProxy = new List<SceneObject> { targetObj } };
            m_testScene.Layers.Add(layer);

            _sceneManager.RegisterScene(m_testScene);
            _sceneManager.SetCurrentScene(m_testScene);

            // Act
            var result = _sceneManager.GetObjectByName("FindMe");

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.EqualTo(targetObj));
        }

        [Test]
        public void GetObjectByName_CurrentScene_Null_ShouldReturnNull()
        {
            // Arrange
            var targetObj = new GameObject { Name = "FindMe" };
            var layer = new GraphicsLayer { SceneObjectsProxy = new List<SceneObject> { targetObj } };
            m_testScene.Layers.Add(layer);

            _sceneManager.RegisterScene(m_testScene);

            // Act
            var result = _sceneManager.GetObjectByName("FindMe");

            // Assert
            Assert.That(result, Is.Null);
        }

        [Test]
        public void GetInteractiveObjectByName_ShouldReturnCorrectObject()
        {
            // Arrange
            var targetObj = new GameObject { Name = "FindMe" };
            var layer = new GraphicsLayer { SceneObjectsProxy = new List<SceneObject> { targetObj } };
            m_testScene.Layers.Add(layer);

            _sceneManager.RegisterScene(m_testScene);
            _sceneManager.SetCurrentScene(m_testScene);

            // Act
            var result = _sceneManager.GetInteractiveObjectByName("FindMe");

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.EqualTo(targetObj));
        }

        [Test]
        public void GetInteractiveObjectByName_CurrentScene_Null_ShouldReturnNull()
        {
            // Arrange
            var targetObj = new GameObject { Name = "FindMe" };
            var layer = new GraphicsLayer { SceneObjectsProxy = new List<SceneObject> { targetObj } };
            m_testScene.Layers.Add(layer);

            _sceneManager.RegisterScene(m_testScene);

            // Act
            var result = _sceneManager.GetInteractiveObjectByName("FindMe");

            // Assert
            Assert.That(result, Is.Null);
        }

        [Test]
        public void GetButtonObjectByName_ShouldReturnCorrectObject()
        {
            // Arrange
            var targetObj = new ButtonObject { Name = "FindMe" };
            var layer = new GraphicsLayer { SceneObjectsProxy = new List<SceneObject> { targetObj } };
            m_testScene.Layers.Add(layer);

            _sceneManager.RegisterScene(m_testScene);
            _sceneManager.SetCurrentScene(m_testScene);

            // Act
            var result = _sceneManager.GetButtonByName("FindMe");

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.EqualTo(targetObj));
        }

        [Test]
        public void GetButtonObjectByName_CurrentScene_Null_ShouldReturnNull()
        {
            // Arrange
            var targetObj = new ButtonObject { Name = "FindMe" };
            var layer = new GraphicsLayer { SceneObjectsProxy = new List<SceneObject> { targetObj } };
            m_testScene.Layers.Add(layer);

            _sceneManager.RegisterScene(m_testScene);

            // Act
            var result = _sceneManager.GetButtonByName("FindMe");

            // Assert
            Assert.That(result, Is.Null);
        }

        [Test]
        public void GetTextLabelByName_ShouldReturnCorrectObject()
        {
            // Arrange
            var targetObj = new TextLabel { Name = "FindMe" };
            var layer = new GraphicsLayer { SceneObjectsProxy = new List<SceneObject> { targetObj } };
            m_testScene.Layers.Add(layer);

            _sceneManager.RegisterScene(m_testScene);
            _sceneManager.SetCurrentScene(m_testScene);

            // Act
            var result = _sceneManager.GetTextLabelByName("FindMe");

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.EqualTo(targetObj));
        }

        [Test]
        public void GetTextLabelByName_CurrentScene_Null_ShouldReturnNull()
        {
            // Arrange
            var targetObj = new TextLabel { Name = "FindMe" };
            var layer = new GraphicsLayer { SceneObjectsProxy = new List<SceneObject> { targetObj } };
            m_testScene.Layers.Add(layer);

            _sceneManager.RegisterScene(m_testScene);

            // Act
            var result = _sceneManager.GetTextLabelByName("FindMe");

            // Assert
            Assert.That(result, Is.Null);
        }

        [Test]
        public void FreeAScene_ShouldRemoveSceneAndFireEvent()
        {
            // Arrange
            _sceneManager.RegisterScene(m_testScene);
            bool removedEventFired = false;
            _sceneManager.SceneRemoved += (scene) => removedEventFired = true;

            // Act
            _sceneManager.FreeAScene("MenuScene");

            // Assert
            Assert.That(_sceneManager.Scenes, Is.Empty);
            Assert.That(removedEventFired, Is.True);
        }

        [Test]
        public void FreeASceneByID_ShouldRemoveSceneAndFireEvent()
        {
            // Arrange
            _sceneManager.RegisterScene(m_testScene);
            bool removedEventFired = false;
            _sceneManager.SceneRemoved += (scene) => removedEventFired = true;

            // Act
            _sceneManager.FreeASceneByID(m_testScene.Id);

            // Assert
            Assert.That(_sceneManager.Scenes, Is.Empty);
            Assert.That(removedEventFired, Is.True);
        }

        [Test]
        public void Clear_ShouldRemoveAllScenesAndNotify()
        {
            // Arrange
            _sceneManager.RegisterScene(new Scene { Id = Guid.NewGuid(), Name = "S1" });
            _sceneManager.RegisterScene(new Scene { Id = Guid.NewGuid(), Name = "S2" });
            int removedCount = 0;
            _sceneManager.SceneRemoved += (scene) => removedCount++;

            // Act
            _sceneManager.Clear();

            // Assert
            Assert.That(_sceneManager.Scenes, Is.Empty);
            Assert.That(removedCount, Is.EqualTo(2));
        }
    }
}