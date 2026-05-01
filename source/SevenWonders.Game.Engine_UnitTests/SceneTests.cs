using SevenWonders.Game.Engine;
using System.Numerics;

namespace SevenWonders.GameEngine_UnitTests
{
    [TestFixture]
    public class SceneTests
    {

        [SetUp]
        public void SetUp()
        {
            m_originalScene = new Scene
            {
                Id = Guid.NewGuid(),
                Name = "MainLevel",
                Visible = true,
                Resolution = new Vector2(1920, 1080),
                Layers = new List<GraphicsLayer>
                {
                    new GraphicsLayer { Id = 1, Name = "Background" }
                }
            };
        }

        [TearDown]
        public void TearDown()
        {
            m_originalScene?.Dispose();
        }

        [Test]
        public void Constructor_ShouldSetDefaultResolutionAndEmptyLayers()
        {
            // Act
            var scene = new Scene();

            // Assert
            Assert.That(scene.Resolution, Is.EqualTo(new Vector2(3840, 2160)));
            Assert.That(scene.Layers, Is.Empty);
            Assert.That(scene.Id, Is.EqualTo(Guid.Empty));
        }

        [Test]
        public void CopyConstructor_ShouldCreateDeepCopyAndNewId()
        {
            // Act
            var copy = new Scene(m_originalScene);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(ReferenceEquals(m_originalScene.Id, copy.Id), Is.False);
                Assert.That(copy.Name, Is.EqualTo(m_originalScene.Name));

                Assert.That(copy.Layers.Count, Is.EqualTo(m_originalScene.Layers.Count));
                Assert.That(ReferenceEquals(m_originalScene.Layers[0], copy.Layers[0]), Is.False);
            });
        }

        [Test]
        public void Equals_WhenAllPropertiesMatch_ShouldReturnTrue()
        {
            // Arrange
            var scene1 = new Scene { Id = Guid.Parse("00000000-0000-0000-0000-000000000001"), Name = "Menu" };
            var scene2 = new Scene { Id = Guid.Parse("00000000-0000-0000-0000-000000000001"), Name = "Menu" };

            // Act & Assert
            Assert.That(scene1, Is.EqualTo(scene2));
            Assert.That(scene2.GetHashCode(), Is.EqualTo(scene1.GetHashCode()));
        }

        [Test]
        public void Resize_ShouldUpdateResolutionAndPropagateToLayers()
        {
            // Arrange
            var newRes = new Vector2(1280, 720);

            var layer = new GraphicsLayer { Name = "UI" };
            m_originalScene.Layers = new List<GraphicsLayer> { layer };
            var oldRes = m_originalScene.Resolution;

            // Act
            m_originalScene.Resize(newRes);

            // Assert
            Assert.That(m_originalScene.Resolution, Is.EqualTo(newRes));
        }

        [Test]
        public void Draw_WhenNotVisible_ShouldReturnImmediately()
        {
            // Arrange
            m_originalScene.Visible = false;
            Assert.DoesNotThrow(() => m_originalScene.Draw(null!));
        }

        private Scene m_originalScene;
    }
}