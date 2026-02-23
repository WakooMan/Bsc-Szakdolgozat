using NUnit.Framework;
using System;
using System.Collections.Generic;
using SevenWonders.GameEngine;
using System.Linq;
using SkiaSharp.Views.Maui;
using NSubstitute;

namespace SevenWonders.GameEngine_UnitTests
{
    [TestFixture]
    public class SceneManagerTests
    {
        private SceneManager _sceneManager;
        private Scene _testScene;

        [SetUp]
        public void SetUp()
        {
            _sceneManager = new SceneManager();
            _testScene = new Scene { Id = Guid.NewGuid(), Name = "MenuScene" };
        }

        [Test]
        public void RegisterScene_ShouldAddSceneAndFireEvent()
        {
            // Arrange
            bool eventFired = false;
            _sceneManager.SceneRegistered += (scene) => eventFired = true;

            // Act
            _sceneManager.RegisterScene(_testScene);

            // Assert
            Assert.That(_sceneManager.Scenes.Count, Is.EqualTo(1));
            Assert.That(eventFired, Is.True);
        }

        [Test]
        public void RegisterScene_ShouldNotAddDuplicateScene()
        {
            // Act
            _sceneManager.RegisterScene(_testScene);
            _sceneManager.RegisterScene(_testScene); // Duplikált próbálkozás

            // Assert
            Assert.That(_sceneManager.Scenes.Count, Is.EqualTo(1));
        }

        [Test]
        public void GetScene_Success()
        {
            // Act
            _sceneManager.RegisterScene(_testScene);
            Scene scene = _sceneManager.GetScene(_testScene.Id);

            // Assert
            Assert.That(scene, Is.EqualTo(_testScene));
        }

        [Test]
        public void GetScene_Throws_Exception()
        {
            Assert.Throws<InvalidOperationException>(() => _sceneManager.GetScene(_testScene.Id));
        }

        [Test]
        public void GetSceneByName_Success()
        {
            // Act
            _sceneManager.RegisterScene(_testScene);
            Scene scene = _sceneManager.GetSceneByName(_testScene.Name);

            // Assert
            Assert.That(scene, Is.EqualTo(_testScene));
        }

        [Test]
        public void GetSceneByName_Throws_Exception()
        {
            Assert.Throws<InvalidOperationException>(() => _sceneManager.GetSceneByName(_testScene.Name));
        }

        [Test]
        public void SetCurrentScene_ShouldWork_IfSceneIsRegistered()
        {
            // Arrange
            _sceneManager.RegisterScene(_testScene);

            // Act
            _sceneManager.SetCurrentScene(_testScene);

            // Assert
            Assert.That(_testScene, Is.EqualTo(_sceneManager.CurrentScene));
        }

        [Test]
        public void SetCurrentScene_ShouldThrowException_IfSceneNotRegistered()
        {
            // Act & Assert
            // Az ArgumentChecker-ed miatt InvalidOperationException-t vagy ArgumentException-t várunk
            Assert.Throws<ArgumentException>(() => _sceneManager.SetCurrentScene(_testScene));
        }

        [Test]
        public void GetObjectByName_ShouldReturnCorrectObject()
        {
            // Arrange
            var targetObj = new GameObject { Name = "FindMe" };
            var layer = new GraphicsLayer { ObjectList = new List<GameObject> { targetObj } };
            _testScene.Layers.Add(layer);

            _sceneManager.RegisterScene(_testScene);
            _sceneManager.SetCurrentScene(_testScene);

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
            var layer = new GraphicsLayer { ObjectList = new List<GameObject> { targetObj } };
            _testScene.Layers.Add(layer);

            _sceneManager.RegisterScene(_testScene);

            // Act
            var result = _sceneManager.GetObjectByName("FindMe");

            // Assert
            Assert.That(result, Is.Null);
        }

        [Test]
        public void FreeAScene_ShouldRemoveSceneAndFireEvent()
        {
            // Arrange
            _sceneManager.RegisterScene(_testScene);
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
            _sceneManager.RegisterScene(_testScene);
            bool removedEventFired = false;
            _sceneManager.SceneRemoved += (scene) => removedEventFired = true;

            // Act
            _sceneManager.FreeASceneByID(_testScene.Id);

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