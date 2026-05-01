using NUnit.Framework;
using System.Numerics;
using SevenWonders.Game.Engine;

namespace SevenWonders.GameEngine_UnitTests
{
    [TestFixture]
    public class ChildTextureTests
    {
        [Test]
        public void Constructor_ShouldInitializeWithDefaultValues()
        {
            // Act
            var ct = new ChildTexture();

            // Assert
            Assert.That(ct.TextureId, Is.EqualTo(-1));
            Assert.That(ct.WidthPercent, Is.EqualTo(0f));
            Assert.That(ct.HeightPercent, Is.EqualTo(0f));
            Assert.That(ct.PositionPercent, Is.EqualTo(Vector2.Zero));
        }

        [Test]
        public void CopyConstructor_ShouldCreateDeepCopy()
        {
            // Arrange
            var original = new ChildTexture
            {
                WidthPercent = 0.5f,
                HeightPercent = 0.3f,
                PositionPercent = new Vector2(0.1f, -0.2f),
                TextureId = 42
            };

            // Act
            var copy = new ChildTexture(original);

            // Assert
            Assert.That(copy.WidthPercent, Is.EqualTo(original.WidthPercent));
            Assert.That(copy.HeightPercent, Is.EqualTo(original.HeightPercent));
            Assert.That(copy.PositionPercent, Is.EqualTo(original.PositionPercent));
            Assert.That(copy.TextureId, Is.EqualTo(original.TextureId));
        }

        [Test]
        public void Equals_SameValues_ShouldReturnTrue()
        {
            // Arrange
            var ct1 = new ChildTexture
            {
                WidthPercent = 0.5f,
                HeightPercent = 0.3f,
                PositionPercent = new Vector2(0.1f, 0.2f),
                TextureId = 7
            };
            var ct2 = new ChildTexture
            {
                WidthPercent = 0.5f,
                HeightPercent = 0.3f,
                PositionPercent = new Vector2(0.1f, 0.2f),
                TextureId = 7
            };

            // Assert
            Assert.That(ct1, Is.EqualTo(ct2));
            Assert.That(ct1.GetHashCode(), Is.EqualTo(ct2.GetHashCode()));
        }

        [Test]
        public void Equals_DifferentValues_ShouldReturnFalse()
        {
            // Arrange
            var ct1 = new ChildTexture
            {
                WidthPercent = 0.5f,
                HeightPercent = 0.3f,
                PositionPercent = new Vector2(0.1f, 0.2f),
                TextureId = 7
            };
            var ct2 = new ChildTexture
            {
                WidthPercent = 0.8f,
                HeightPercent = 0.3f,
                PositionPercent = new Vector2(0.1f, 0.2f),
                TextureId = 7
            };

            // Assert
            Assert.That(ct1.Equals(ct2), Is.False);
        }

        [Test]
        public void Equals_Null_ShouldReturnFalse()
        {
            // Arrange
            var ct = new ChildTexture { WidthPercent = 0.5f };
            ChildTexture? other = null;

            // Assert
            Assert.That(ct.Equals(other), Is.False);
        }

        [Test]
        public void Equals_Object_NotChildTexture_ShouldReturnFalse()
        {
            // Arrange
            var ct = new ChildTexture { WidthPercent = 0.5f };
            object? other = new object();

            // Assert
            Assert.That(ct.Equals(other), Is.False);
        }

        [Test]
        public void Equals_Object_ChildTexture_ShouldReturnTrue()
        {
            // Arrange
            var ct1 = new ChildTexture
            {
                WidthPercent = 0.5f,
                HeightPercent = 0.3f,
                PositionPercent = new Vector2(0.1f, 0.2f),
                TextureId = 7
            };
            object ct2 = new ChildTexture
            {
                WidthPercent = 0.5f,
                HeightPercent = 0.3f,
                PositionPercent = new Vector2(0.1f, 0.2f),
                TextureId = 7
            };

            // Assert
            Assert.That(ct1.Equals(ct2), Is.True);
        }

        [Test]
        public void Equals_DifferentTextureId_ShouldReturnFalse()
        {
            // Arrange
            var ct1 = new ChildTexture
            {
                WidthPercent = 0.5f,
                TextureId = 1
            };
            var ct2 = new ChildTexture
            {
                WidthPercent = 0.5f,
                TextureId = 2
            };

            // Assert
            Assert.That(ct1.Equals(ct2), Is.False);
        }
    }
}
