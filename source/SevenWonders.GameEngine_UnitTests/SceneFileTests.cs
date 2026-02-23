using SevenWonders.GameEngine;

namespace SevenWonders.GameEngine_UnitTests
{
    [TestFixture]
    public class SceneFileTests
    {
        [Test]
        public void When_Default_Constructor_Called()
        {
            SceneFile sceneFile = new SceneFile();

            Assert.That(sceneFile, Is.Not.Null);
            Assert.That(sceneFile.Name, Is.EqualTo(string.Empty));
            Assert.That(sceneFile.Stream, Is.EqualTo(Stream.Null));
        }

        [Test]
        public void When_Constructor_Called()
        {
            Stream stream = new MemoryStream();
            string name = "test";

            SceneFile sceneFile = new SceneFile(name, stream);

            Assert.That(sceneFile, Is.Not.Null);
            Assert.That(sceneFile.Name, Is.EqualTo(name));
            Assert.That(sceneFile.Stream, Is.EqualTo(stream));
        }
    }
}
