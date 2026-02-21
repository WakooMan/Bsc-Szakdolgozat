using NUnit.Framework;
using System;
using System.Collections.Generic;
using SevenWonders.GameEngine;
using System.Linq;

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
            Assert.AreEqual(1, _sceneManager.Scenes.Count);
            Assert.IsTrue(eventFired, "A SceneRegistered eseménynek le kell futnia regisztrációkor.");
        }

        [Test]
        public void RegisterScene_ShouldNotAddDuplicateScene()
        {
            // Act
            _sceneManager.RegisterScene(_testScene);
            _sceneManager.RegisterScene(_testScene); // Duplikált próbálkozás

            // Assert
            Assert.AreEqual(1, _sceneManager.Scenes.Count, "A jelenetet nem szabad kétszer hozzáadni.");
        }

        [Test]
        public void SetCurrentScene_ShouldWork_IfSceneIsRegistered()
        {
            // Arrange
            _sceneManager.RegisterScene(_testScene);

            // Act
            _sceneManager.SetCurrentScene(_testScene);

            // Assert
            Assert.AreEqual(_testScene, _sceneManager.CurrentScene);
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
            Assert.IsNotNull(result);
            Assert.AreEqual("FindMe", result.Name);
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
            Assert.IsEmpty(_sceneManager.Scenes);
            Assert.IsTrue(removedEventFired);
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
            Assert.IsEmpty(_sceneManager.Scenes);
            Assert.AreEqual(2, removedCount);
        }
    }
}