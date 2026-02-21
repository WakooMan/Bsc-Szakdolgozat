using NSubstitute;
using SevenWonders.Common;
using SevenWonders.GameEngine;

namespace SevenWonders.GameEngine_UnitTests
{
    [TestFixture]
    public class SceneLoaderTests
    {
        private IXmlHandler _xmlHandler;
        private IZipFileReceiver _zipReceiver;
        private SceneLoader _sceneLoader;
        private string _testRoot;

        [SetUp]
        public void SetUp()
        {
            _xmlHandler = Substitute.For<IXmlHandler>();
            _zipReceiver = Substitute.For<IZipFileReceiver>();
            _sceneLoader = new SceneLoader(_xmlHandler, _zipReceiver);

            // A tesztkörnyezet tisztán tartása érdekében
            _testRoot = Path.Combine(Directory.GetCurrentDirectory(), "ScenesTemp");
        }

        [TearDown]
        public void Cleanup()
        {
            // Teszt után töröljük az ideiglenes mappákat, ha léteznek
            if (Directory.Exists(_testRoot))
                Directory.Delete(_testRoot, true);
        }

        [Test]
        public void Constructor_ShouldThrowArgumentNullException_WhenXmlHandlerIsNull()
        {
            // Assert & Act
            Assert.Throws<ArgumentNullException>(() => new SceneLoader(null!, _zipReceiver));
        }

        [Test]
        public async Task LoadScenes_ShouldReturnEmptyList_WhenNoZipFilesFound()
        {
            // Arrange
            _zipReceiver.ReceiveZipFiles().Returns(Task.FromResult<ICollection<SceneFile>>(new List<SceneFile>()));

            // Act
            var result = await _sceneLoader.LoadScenes();

            // Assert
            Assert.That(result, Is.Empty);
        }

        [Test]
        public void ReceiveSceneFolder_ShouldReturnCorrectPath()
        {
            // Arrange
            var scene = new Scene { Name = "Egipt" };

            // Act
            var path = _sceneLoader.ReceiveSceneFolder(scene);

            // Assert
            Assert.That(path.EndsWith("Egipt"), Is.True);
            Assert.That(path.Contains("ScenesTemp"), Is.True);
        }

        [Test]
        public void SaveScene_ShouldThrowException_WhenSceneIsNull()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => _sceneLoader.SaveScene(null));
        }

        [Test]
        public void SaveScene_ShouldCallXmlHandlerSerialize()
        {
            // Arrange
            var scene = new Scene { Name = "Alexandria", Id = Guid.NewGuid() };
            // Létrehozzuk a temp mappát, hogy a checkForSceneFolder ne bukjon el
            string sceneDir = Path.Combine(_testRoot, "Alexandria");
            Directory.CreateDirectory(sceneDir);

            // Act
            _sceneLoader.SaveScene(scene, checkForSceneFolder: true);

            // Assert
            _xmlHandler.Received(1).Serialize(
                Arg.Is<string>(s => s.Contains("scene.xml")),
                scene
            );
        }
    }
}