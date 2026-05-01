using NUnit.Framework;
using System.Numerics;
using SevenWonders.Game.Engine;

namespace SevenWonders.GameEngine_UnitTests
{
    [TestFixture]
    public class SpriteFrameTests
    {
        [Test]
        public void Constructor_ShouldInitializeWithDefaultValues()
        {
            // Act
            var sf = new SpriteFrame();

            // Assert
            Assert.That(sf.Name, Is.EqualTo(string.Empty));
            Assert.That(sf.TextureId, Is.EqualTo(0));
            Assert.That(sf.Left, Is.EqualTo(0));
            Assert.That(sf.Bottom, Is.EqualTo(0));
        }

        [Test]
        public void CopyConstructor_ShouldCreateDeepCopy()
        {
            // Arrange
            var original = new SpriteFrame
            {
                Name = "Run_01",
                Left = 0,
                Top = 0,
                Right = 32,
                Bottom = 32,
                TextureId = 10
            };

            // Act
            var copy = new SpriteFrame(original);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(copy.Name, Is.EqualTo(original.Name));
                Assert.That(copy.Left, Is.EqualTo(original.Left));
                Assert.That(copy.Right, Is.EqualTo(original.Right));
                Assert.That(copy.TextureId, Is.EqualTo(original.TextureId));
            });
        }

        [Test]
        public void Equals_WhenPropertiesMatch_ShouldReturnTrue()
        {
            // Arrange
            var sf1 = new SpriteFrame { Name = "Idle", Left = 10 };
            var sf2 = new SpriteFrame { Name = "Idle", Left = 10 };

            // Act & Assert
            Assert.That(sf1, Is.EqualTo(sf2));
            Assert.That(sf1.GetHashCode(), Is.EqualTo(sf2.GetHashCode()));
        }

        [Test]
        public void Equals_WhenTextureDiffers_ShouldReturnFalse()
        {
            // Arrange
            var sf1 = new SpriteFrame { Name = "Frame", TextureId = 1 };
            var sf2 = new SpriteFrame { Name = "Frame", TextureId = 2 };

            // Act & Assert
            Assert.That(sf1, Is.Not.EqualTo(sf2));
        }

        [Test]
        public void Equals_SpriteFrame_Null_ShouldReturnFalse()
        {
            // Arrange
            var sf = new SpriteFrame { Name = "Frame" };
            SpriteFrame? other = null;

            // Act & Assert
            Assert.That(sf.Equals(other), Is.False);
        }

        [Test]
        public void Equals_Object_NotSpriteFrame_ShouldReturnFalse()
        {
            // Arrange
            var sf = new SpriteFrame { Name = "Frame" };
            object? other = new object();

            // Act & Assert
            Assert.That(sf.Equals(other), Is.False);
        }
    }
}