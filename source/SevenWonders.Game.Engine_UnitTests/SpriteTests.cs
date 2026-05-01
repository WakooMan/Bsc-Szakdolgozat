using SevenWonders.Game.Engine;
using System.Numerics;

namespace SevenWonders.GameEngine_UnitTests
{
    [TestFixture]
    public class SpriteTests
    {
        [Test]
        public void CopyConstructor_ShouldCreateDeepCopyOfFrames()
        {
            // Arrange
            var original = new Sprite
            {
                Name = "RunAnimation",
                NumFrames = 2,
                Frames = new List<SpriteFrame>
                {
                    new SpriteFrame { },
                    new SpriteFrame { }
                },
                Children = new List<ChildObject>()
            };

            ChildTexture childTexture = new ChildTexture
            {
                TextureId = 20,
                WidthPercent = 0.5f,
                HeightPercent = 0.5f,
                PositionPercent = new Vector2(0, 0)
            };

            original.AddChildObject(childTexture);

            // Act
            var copy = new Sprite(original);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(copy.Name, Is.EqualTo(original.Name));
                Assert.That(original.Frames.Count, Is.EqualTo(copy.Frames.Count));
                Assert.That(ReferenceEquals(copy.Frames, original.Frames), Is.False);
                Assert.That(ReferenceEquals(original.Frames[0], copy.Frames[0]), Is.False);
                Assert.That(original.Children.Count, Is.EqualTo(1));
                Assert.That(ReferenceEquals(original.Children[0], copy.Children[0]), Is.False);
                Assert.That(copy.Children[0], Is.TypeOf<ChildTexture>());
                Assert.That(((ChildTexture)copy.Children[0]).TextureId, Is.EqualTo(20));
                Assert.That(copy.Children[0].WidthPercent, Is.EqualTo(0.5f));
                Assert.That(copy.Children[0].HeightPercent, Is.EqualTo(0.5f));
                Assert.That(copy.Children[0].PositionPercent, Is.EqualTo(new Vector2(0, 0)));
            });
        }

        [Test]
        public void Equals_WhenDataMatches_ShouldReturnTrue()
        {
            // Arrange
            var s1 = new Sprite { Name = "Idle" };
            var s2 = new Sprite { Name = "Idle" };

            // Assert
            Assert.That(s1, Is.EqualTo(s2));
            Assert.That(s1.GetHashCode(), Is.EqualTo(s2.GetHashCode()));
        }

        [Test]
        public void AddChildTexture_ShouldCreateChildWithCorrectPercentageValues()
        {
            // Arrange
            var sprite = new Sprite { Name = "Frame_01" };

            // Act
            ChildTexture childTexture = new ChildTexture
            {
                TextureId = 42,
                WidthPercent = 0.25f,
                HeightPercent = 0.5f,
                PositionPercent = new Vector2(0.1f, -0.2f)
            };

            sprite.AddChildObject(childTexture);
            ChildObject child = sprite.Children.First();

            // Assert
            Assert.That(sprite.Children.Count, Is.EqualTo(1));
            Assert.That(child, Is.TypeOf<ChildTexture>());
            Assert.That(((ChildTexture)child).TextureId, Is.EqualTo(42));
            Assert.That(child.WidthPercent, Is.EqualTo(0.25f));
            Assert.That(child.HeightPercent, Is.EqualTo(0.5f));
            Assert.That(child.PositionPercent.X, Is.EqualTo(0.1f));
            Assert.That(child.PositionPercent.Y, Is.EqualTo(-0.2f));
        }

        [Test]
        public void AddChildTexture_ShouldAddMultipleChildren()
        {
            // Arrange
            var sprite = new Sprite { Name = "Frame_01" };

            // Act

            ChildTexture childTexture1 = new ChildTexture
            {
                TextureId = 1,
                WidthPercent = 0.5f,
                HeightPercent = 0.5f,
                PositionPercent = new Vector2(0, 0)
            };

            ChildTexture childTexture2 = new ChildTexture
            {
                TextureId = 2,
                WidthPercent = 0.3f,
                HeightPercent = 0.3f,
                PositionPercent = new Vector2(0.2f, 0.2f)
            };

            sprite.AddChildObject(childTexture1);
            sprite.AddChildObject(childTexture2);

            // Assert
            Assert.That(sprite.Children.Count, Is.EqualTo(2));
            Assert.That(sprite.Children[0], Is.TypeOf<ChildTexture>());
            Assert.That(((ChildTexture)sprite.Children[0]).TextureId, Is.EqualTo(1));
            Assert.That(sprite.Children[1], Is.TypeOf<ChildTexture>());
            Assert.That(((ChildTexture)sprite.Children[1]).TextureId, Is.EqualTo(2));
        }

        [Test]
        public void Equals_DifferentChildTextures_ShouldReturnFalse()
        {
            // Arrange
            var sprite1 = new Sprite { Name = "Frame" };
            ChildTexture childTexture = new ChildTexture
            {
                TextureId = 1,
                WidthPercent = 0.5f,
                HeightPercent = 0.5f,
                PositionPercent = new Vector2(0, 0)
            };
            sprite1.AddChildObject(childTexture);
            var sprite2 = new Sprite { Name = "Frame" };

            // Act & Assert
            Assert.That(sprite1.Equals(sprite2), Is.False);
        }

        [Test]
        public void Equals_SameChildTextures_ShouldReturnTrue()
        {
            // Arrange
            var sf1 = new Sprite { Name = "Frame" };
            ChildTexture childTexture1 = new ChildTexture
            {
                TextureId = 1,
                WidthPercent = 0.5f,
                HeightPercent = 0.5f,
                PositionPercent = new Vector2(0, 0)
            };
            sf1.AddChildObject(childTexture1);

            ChildTexture childTexture2 = new ChildTexture
            {
                TextureId = 1,
                WidthPercent = 0.5f,
                HeightPercent = 0.5f,
                PositionPercent = new Vector2(0, 0)
            };
            var sf2 = new Sprite { Name = "Frame" };
            sf2.AddChildObject(childTexture2);

            // Act & Assert
            Assert.That(sf1.Equals(sf2), Is.True);
            Assert.That(sf1.GetHashCode(), Is.EqualTo(sf2.GetHashCode()));
        }

    }
}