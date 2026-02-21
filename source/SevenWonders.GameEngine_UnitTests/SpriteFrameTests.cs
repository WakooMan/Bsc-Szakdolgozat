using NUnit.Framework;
using System.Numerics;
using SevenWonders.GameEngine;

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
            Assert.That(sf.Frame, Is.Not.Null);
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
                Frame = new Texture { FileName = "spritesheet.png" }
            };

            // Act
            var copy = new SpriteFrame(original);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(copy.Name, Is.EqualTo(original.Name));
                Assert.That(copy.Left, Is.EqualTo(original.Left));
                Assert.That(copy.Right, Is.EqualTo(original.Right));

                // Deep copy ellenőrzése a Texture objektumra
                Assert.That(copy.Frame, Is.Not.EqualTo(original.Frame));
                Assert.That(original.Frame.FileName, Is.EqualTo(copy.Frame.FileName));
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
            var sf1 = new SpriteFrame { Name = "Frame", Frame = new Texture { FileName = "a.png" } };
            var sf2 = new SpriteFrame { Name = "Frame", Frame = new Texture { FileName = "b.png" } };

            // Act & Assert
            Assert.That(sf1, Is.Not.EqualTo(sf2));
        }
    }
}