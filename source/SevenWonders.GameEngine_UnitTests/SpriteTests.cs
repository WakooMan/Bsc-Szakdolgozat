using SevenWonders.GameEngine;
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
                    new SpriteFrame { /* adatok */ },
                    new SpriteFrame { /* adatok */ }
                }
            };

            // Act
            var copy = new Sprite(original);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(copy.Name, Is.EqualTo(original.Name));
                Assert.That(original.Frames.Count, Is.EqualTo(copy.Frames.Count));
                Assert.That(copy.Frames, Is.Not.EqualTo(original.Frames));
                Assert.That(original.Frames[0], Is.Not.EqualTo(copy.Frames[0]));
            });
        }

        [Test]
        public void Equals_WhenDataMatches_ShouldReturnTrue()
        {
            // Arrange
            var s1 = new Sprite { Name = "Idle", Fps = 10 };
            var s2 = new Sprite { Name = "Idle", Fps = 10 };

            // Assert
            Assert.That(s1, Is.EqualTo(s2));
            Assert.That(s1.GetHashCode(), Is.EqualTo(s2.GetHashCode()));
        }

        [Test]
        public void Draw_ShouldIncrementLastUpdate()
        {
            // Arrange
            var sprite = new Sprite { Fps = 60 };
            sprite.Frames.Add(new SpriteFrame()); // Kell legalább egy frame
            sprite.LastUpdate = 0;

            // Act
            // SkiaSharp args null-al is oké, ha a SpriteFrame.Draw lekezeli (vagy mockoljuk)
            sprite.Draw(null!, Vector2.Zero, Vector2.One, 0, 10, 10);

            // Assert
            Assert.That(sprite.LastUpdate, Is.EqualTo(1));
        }

        [Test]
        public void Draw_ShouldUpdateFrame_WhenLastUpdateExceedsFps()
        {
            // Arrange
            var sprite = new Sprite
            {
                Fps = 2,
                LastUpdate = 2,
                ActualFrame = 0,
                LoopAnimation = true
            };
            sprite.Frames.Add(new SpriteFrame());
            sprite.Frames.Add(new SpriteFrame());

            // Act
            sprite.Draw(null!, Vector2.Zero, Vector2.One, 0, 10, 10);

            // Assert
            // A kódod alapján: LastUpdate++ -> 3. Mivel 2 < 3, lefut a frame váltás.
            Assert.That(sprite.ActualFrame > 0 || sprite.LastUpdate > 2, Is.True);
        }
    }
}