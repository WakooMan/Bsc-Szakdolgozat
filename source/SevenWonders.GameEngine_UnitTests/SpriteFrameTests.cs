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
            Assert.AreEqual(string.Empty, sf.Name);
            Assert.IsNotNull(sf.Frame);
            Assert.AreEqual(0, sf.Left);
            Assert.AreEqual(0, sf.Bottom);
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
                Assert.AreEqual(original.Name, copy.Name);
                Assert.AreEqual(original.Left, copy.Left);
                Assert.AreEqual(original.Right, copy.Right);

                // Deep copy ellenőrzése a Texture objektumra
                Assert.AreNotSame(original.Frame, copy.Frame);
                Assert.AreEqual(original.Frame.FileName, copy.Frame.FileName);
            });
        }

        [Test]
        public void Equals_WhenPropertiesMatch_ShouldReturnTrue()
        {
            // Arrange
            var sf1 = new SpriteFrame { Name = "Idle", Left = 10 };
            var sf2 = new SpriteFrame { Name = "Idle", Left = 10 };

            // Act & Assert
            Assert.IsTrue(sf1.Equals(sf2));
            Assert.AreEqual(sf1.GetHashCode(), sf2.GetHashCode());
        }

        [Test]
        public void Equals_WhenTextureDiffers_ShouldReturnFalse()
        {
            // Arrange
            var sf1 = new SpriteFrame { Name = "Frame", Frame = new Texture { FileName = "a.png" } };
            var sf2 = new SpriteFrame { Name = "Frame", Frame = new Texture { FileName = "b.png" } };

            // Act & Assert
            Assert.IsFalse(sf1.Equals(sf2));
        }
    }
}