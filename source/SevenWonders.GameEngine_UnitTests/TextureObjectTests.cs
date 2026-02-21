using SevenWonders.GameEngine;
using System.Numerics;

namespace SevenWonders.GameEngine_UnitTests
{
    [TestFixture]
    public class TextureObjectTests
    {
        [Test]
        public void Constructor_ShouldSetDefaultValues()
        {
            // Act
            var obj = new TextureObject();

            // Assert
            Assert.That(obj.Name, Is.Not.EqualTo(string.Empty));
            Assert.That(obj.Texture, Is.Not.Null);
            Assert.That(obj.Width, Is.EqualTo(0));
            Assert.That(obj.Height, Is.EqualTo(0));
        }

        [Test]
        public void CopyConstructor_ShouldCreateDeepCopy()
        {
            // Arrange
            var original = new TextureObject
            {
                Name = "Background",
                Width = 1920,
                Height = 1080,
                Position = new Vector2(10, 20),
                Visible = true
            };

            // Act
            var copy = new TextureObject(original);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(original.Name, Is.EqualTo(copy.Name));
                Assert.That(original.Width, Is.EqualTo(copy.Width));
                Assert.That(original.Position, Is.EqualTo(copy.Position));
                Assert.That(original.Visible, Is.EqualTo(copy.Visible));
                // Ellenőrizzük, hogy a Texture is új példány-e (ha a Texture copy-konstruktora jól működik)
                Assert.That(original.Texture, Is.Not.EqualTo(copy.Texture));
            });
        }

        [Test]
        public void Resize_ShouldScalePositionAndDimensionsCorrectly()
        {
            // Arrange
            var obj = new TextureObject
            {
                Position = new Vector2(100, 200),
                Width = 50,
                Height = 50
            };
            var oldRes = new Vector2(800, 600);
            var newRes = new Vector2(1600, 1200); // 2x szorzó mindkét irányba

            // Act
            obj.Resize(oldRes, newRes);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(obj.Position.X, Is.EqualTo(200f));
                Assert.That(obj.Position.Y, Is.EqualTo(400f));
                Assert.That(obj.Width, Is.EqualTo(100f));
                Assert.That(obj.Height, Is.EqualTo(100f));
            });
        }

        [Test]
        public void Equals_WhenPropertiesMatch_ShouldReturnTrue()
        {
            // Arrange
            var obj1 = new TextureObject { Id = 1, Name = "Stone" };
            var obj2 = new TextureObject { Id = 1, Name = "Stone" };

            // Act & Assert
            Assert.That(obj1, Is.EqualTo(obj2));
            Assert.That(obj1.GetHashCode(), Is.EqualTo(obj2.GetHashCode()));
        }

        [Test]
        public void Equals_WhenDifferent_ShouldReturnFalse()
        {
            // Arrange
            var obj1 = new TextureObject { Id = 1, Name = "Stone" };
            var obj2 = new TextureObject { Id = 2, Name = "Wood" };

            // Act & Assert
            Assert.That(obj1, Is.EqualTo(obj2));
        }
    }
}