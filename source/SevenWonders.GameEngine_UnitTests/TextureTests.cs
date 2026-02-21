using NUnit.Framework;
using SkiaSharp;
using System.Numerics;
using System.IO;
using SevenWonders.GameEngine;

namespace SevenWonders.GameEngine_UnitTests
{
    [TestFixture]
    public class TextureTests
    {
        private string _tempImagePath;

        [SetUp]
        public void SetUp()
        {
            // Létrehozunk egy valódi, pici bitmap fájlt a LoadTexture teszteléséhez
            _tempImagePath = Path.Combine(Path.GetTempPath(), "test_texture.png");
            using (var bitmap = new SKBitmap(100, 200))
            {
                using (var image = SKImage.FromBitmap(bitmap))
                using (var data = image.Encode(SKEncodedImageFormat.Png, 100))
                using (var stream = File.OpenWrite(_tempImagePath))
                {
                    data.SaveTo(stream);
                }
            }
        }

        [TearDown]
        public void TearDown()
        {
            if (File.Exists(_tempImagePath))
                File.Delete(_tempImagePath);
        }

        [Test]
        public void Constructor_ShouldInitializeCorrectly()
        {
            var texture = new Texture();
            Assert.That(string.Empty, Is.EqualTo(texture.FileName));
        }

        [Test]
        public void CopyConstructor_ShouldPerformDeepCopy()
        {
            // Arrange
            var original = new Texture
            {
                FileName = "card.png",
                OriginalWidth = 50,
                OriginalHeight = 100,
                Color = SKColors.Red
            };

            // Act
            var copy = new Texture(original);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(original.FileName, Is.EqualTo(copy.FileName));
                Assert.That(original.Color, Is.EqualTo(copy.Color));
                Assert.That(original.OriginalWidth, Is.EqualTo(copy.OriginalWidth));
                // String referencia ellenőrzés (bár a string immutábilis)
                Assert.That(original.FileName, Is.Not.EqualTo(copy.FileName));
            });
        }

        [Test]
        public void LoadTexture_ShouldSetOriginalDimensions_WhenValidFileExists()
        {
            // Arrange
            var texture = new Texture { FileName = Path.GetFileName(_tempImagePath) };
            string folder = Path.GetDirectoryName(_tempImagePath)!;

            // Act
            texture.LoadTexture(folder);

            // Assert
            Assert.That(100, Is.EqualTo(texture.OriginalWidth));
            Assert.That(200, Is.EqualTo(texture.OriginalHeight));
        }

        [Test]
        public void Equals_SameAttributes_ShouldReturnTrue()
        {
            // Arrange
            var t1 = new Texture { FileName = "a.png", Color = SKColors.Blue };
            var t2 = new Texture { FileName = "a.png", Color = SKColors.Blue };

            // Assert
            Assert.That(t1, Is.EqualTo(t2));
            Assert.That(t1.GetHashCode(), Is.EqualTo(t2.GetHashCode()));
        }

        [Test]
        public void Draw_ShouldNotThrow_WhenBitmapNotLoaded()
        {
            // Arrange
            var texture = new Texture();
            // Act & Assert
            // Mivel m_bitmap null, a Draw metódusnak az if (m_bitmap == null) miatt 
            // azonnal vissza kell térnie hiba nélkül.
            Assert.DoesNotThrow(() => texture.Draw(null!, Vector2.Zero, Vector2.One, 0, 10, 10));
        }
    }
}