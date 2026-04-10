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
            Assert.That(sf.TextureId, Is.EqualTo(0));
            Assert.That(sf.Left, Is.EqualTo(0));
            Assert.That(sf.Bottom, Is.EqualTo(0));
            Assert.That(sf.ChildTextures, Is.Empty);
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
        public void CopyConstructor_ShouldDeepCopyChildTextures()
        {
            // Arrange
            var original = new SpriteFrame
            {
                Name = "Frame_01",
                TextureId = 10
            };
            original.AddChildTexture(20, 0.5f, 0.5f, new Vector2(0, 0));

            // Act
            var copy = new SpriteFrame(original);

            // Assert
            Assert.That(copy.ChildTextures.Count, Is.EqualTo(1));
            Assert.That(ReferenceEquals(original.ChildTextures[0], copy.ChildTextures[0]), Is.False);
            Assert.That(copy.ChildTextures[0].TextureId, Is.EqualTo(20));
            Assert.That(copy.ChildTextures[0].WidthPercent, Is.EqualTo(0.5f));
            Assert.That(copy.ChildTextures[0].HeightPercent, Is.EqualTo(0.5f));
            Assert.That(copy.ChildTextures[0].PositionPercent, Is.EqualTo(new Vector2(0, 0)));
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
        public void AddChildTexture_ShouldCreateChildWithCorrectPercentageValues()
        {
            // Arrange
            var sf = new SpriteFrame { Name = "Frame_01" };

            // Act
            var child = sf.AddChildTexture(42, 0.25f, 0.5f, new Vector2(0.1f, -0.2f));

            // Assert
            Assert.That(sf.ChildTextures.Count, Is.EqualTo(1));
            Assert.That(child.TextureId, Is.EqualTo(42));
            Assert.That(child.WidthPercent, Is.EqualTo(0.25f));
            Assert.That(child.HeightPercent, Is.EqualTo(0.5f));
            Assert.That(child.PositionPercent.X, Is.EqualTo(0.1f));
            Assert.That(child.PositionPercent.Y, Is.EqualTo(-0.2f));
        }

        [Test]
        public void AddChildTexture_ShouldAddMultipleChildren()
        {
            // Arrange
            var sf = new SpriteFrame { Name = "Frame_01" };

            // Act
            sf.AddChildTexture(1, 0.5f, 0.5f, new Vector2(0, 0));
            sf.AddChildTexture(2, 0.3f, 0.3f, new Vector2(0.2f, 0.2f));

            // Assert
            Assert.That(sf.ChildTextures.Count, Is.EqualTo(2));
            Assert.That(sf.ChildTextures[0].TextureId, Is.EqualTo(1));
            Assert.That(sf.ChildTextures[1].TextureId, Is.EqualTo(2));
        }

        [Test]
        public void Equals_DifferentChildTextures_ShouldReturnFalse()
        {
            // Arrange
            var sf1 = new SpriteFrame { Name = "Frame" };
            sf1.AddChildTexture(1, 0.5f, 0.5f, new Vector2(0, 0));
            var sf2 = new SpriteFrame { Name = "Frame" };

            // Act & Assert
            Assert.That(sf1.Equals(sf2), Is.False);
        }

        [Test]
        public void Equals_SameChildTextures_ShouldReturnTrue()
        {
            // Arrange
            var sf1 = new SpriteFrame { Name = "Frame" };
            sf1.AddChildTexture(1, 0.5f, 0.5f, new Vector2(0, 0));
            var sf2 = new SpriteFrame { Name = "Frame" };
            sf2.AddChildTexture(1, 0.5f, 0.5f, new Vector2(0, 0));

            // Act & Assert
            Assert.That(sf1.Equals(sf2), Is.True);
            Assert.That(sf1.GetHashCode(), Is.EqualTo(sf2.GetHashCode()));
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