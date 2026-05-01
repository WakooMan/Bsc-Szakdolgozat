using SevenWonders.Game.Engine;
using SkiaSharp;
using SkiaSharp.Views.Maui;
using System.Numerics;

namespace SevenWonders.GameEngine_UnitTests
{
    public class GameObjectTests
    {
        [Test]
        public void Constructor_ShouldInitializeDefaults()
        {
            // Act
            var obj = new GameObject();

            // Assert
            Assert.That(string.Empty, Is.EqualTo(obj.Name));
            Assert.That(obj.Animations, Is.Empty);
            Assert.That(0, Is.EqualTo(obj.CurrentAnim));
        }

        [Test]
        public void CopyConstructor_ShouldCreateDeepCopy()
        {
            // Arrange
            var original = new GameObject
            {
                Name = "TestObj",
                Id = 1,
                Position = new Vector2(10, 20),
                VisualSize = new Vector2(1, 1),
                Width = 100,
                Height = 100
            };

            // Act
            var copy = new GameObject(original);

            // Assert
            Assert.That(original.Name, Is.EqualTo(copy.Name));
            Assert.That(original.Id, Is.EqualTo(copy.Id));
            Assert.That(original.Position, Is.EqualTo(copy.Position));
            Assert.That(ReferenceEquals(original, copy), Is.False);
        }

        [Test]
        public void Equals_SameValues_ShouldReturnTrue()
        {
            // Arrange
            var obj1 = new GameObject { Id = 1, Name = "Player" };
            var obj2 = new GameObject { Id = 1, Name = "Player" };

            // Assert
            Assert.That(obj1, Is.EqualTo(obj2));
            Assert.That(obj1.GetHashCode(), Is.EqualTo(obj2.GetHashCode()));
        }

        [Test]
        public void Equals_GameObject_Null_ShouldReturnFalse()
        {
            // Arrange
            var obj1 = new GameObject { Id = 1, Name = "Player" };
            GameObject? obj2 = null;

            // Assert
            Assert.That(obj1.Equals(obj2), Is.False);
        }

        [Test]
        public void Equals_Object_Not_GameObject_ShouldReturnFalse()
        {
            // Arrange
            var obj1 = new GameObject { Id = 1, Name = "Player" };
           object? obj2 = new object();

            // Assert
            Assert.That(obj1.Equals(obj2), Is.False);
        }

        [TestCase(10, 10, true, true, true)]
        [TestCase(5, 5, true, true, false)]
        [TestCase(25, 25, true, true, true)]
        [TestCase(30, 30, true, true, true)]
        [TestCase(31, 31, true, true, false)]
        [TestCase(10, 10, false, true, false)]
        [TestCase(5, 5, false, true, false)]
        [TestCase(25, 25, false, true, false)]
        [TestCase(30, 30, false, true, false)]
        [TestCase(31, 31, false, true, false)]
        [TestCase(10, 10, true, false, false)]
        [TestCase(5, 5, true, false, false)]
        [TestCase(25, 25, true, false, false)]
        [TestCase(30, 30, true, false, false)]
        [TestCase(31, 31, true, false, false)]
        public void When_OnTouchPressed_Called(float touchX, float touchY, bool graphicsLayerVisible, bool gameObjectVisible, bool expectedResult)
        {
            // Arrange
            bool result = false;
            GraphicsLayer graphicsLayer = new GraphicsLayer()
            {
                Visible = graphicsLayerVisible
            };
            SKTouchEventArgs sKTouchEventArgs = new SKTouchEventArgs(1, SKTouchAction.Pressed, new SKPoint(touchX, touchY), true);
            var obj = new GameObject
            {
                Position = new Vector2(20, 20),
                Width = 20,
                Height = 20,
                VisualSize = new Vector2(1, 1),
                Visible = gameObjectVisible
            };
            obj.PressedEvent += (args, e) => result = true;

            // Act

            obj.OnTouchPressed(sKTouchEventArgs, graphicsLayer);

            // Assert
            Assert.That(expectedResult, Is.EqualTo(result));
        }

        [TestCase(10, 10, true, true, true)]
        [TestCase(5, 5, true, true, false)]
        [TestCase(25, 25, true, true, true)]
        [TestCase(30, 30, true, true, true)]
        [TestCase(31, 31, true, true, false)]
        [TestCase(10, 10, false, true, false)]
        [TestCase(5, 5, false, true, false)]
        [TestCase(25, 25, false, true, false)]
        [TestCase(30, 30, false, true, false)]
        [TestCase(31, 31, false, true, false)]
        [TestCase(10, 10, true, false, false)]
        [TestCase(5, 5, true, false, false)]
        [TestCase(25, 25, true, false, false)]
        [TestCase(30, 30, true, false, false)]
        [TestCase(31, 31, true, false, false)]
        public void When_OnTouchMoved_Called(float touchX, float touchY, bool graphicsLayerVisible, bool gameObjectVisible, bool expectedResult)
        {
            // Arrange
            bool result = false;
            GraphicsLayer graphicsLayer = new GraphicsLayer()
            {
                Visible = graphicsLayerVisible
            };
            SKTouchEventArgs sKTouchEventArgs = new SKTouchEventArgs(1, SKTouchAction.Moved, new SKPoint(touchX, touchY), true);
            var obj = new GameObject
            {
                Position = new Vector2(20, 20),
                Width = 20,
                Height = 20,
                VisualSize = new Vector2(1, 1),
                Visible = gameObjectVisible
            };
            obj.MoveEvent += (args, e) => result = true;

            // Act

            obj.OnTouchMoved(sKTouchEventArgs, graphicsLayer);

            // Assert
            Assert.That(expectedResult, Is.EqualTo(result));
        }

        [TestCase(10, 10, true, true, true)]
        [TestCase(5, 5, true, true, false)]
        [TestCase(25, 25, true, true, true)]
        [TestCase(30, 30, true, true, true)]
        [TestCase(31, 31, true, true, false)]
        [TestCase(10, 10, false, true, false)]
        [TestCase(5, 5, false, true, false)]
        [TestCase(25, 25, false, true, false)]
        [TestCase(30, 30, false, true, false)]
        [TestCase(31, 31, false, true, false)]
        [TestCase(10, 10, true, false, false)]
        [TestCase(5, 5, true, false, false)]
        [TestCase(25, 25, true, false, false)]
        [TestCase(30, 30, true, false, false)]
        [TestCase(31, 31, true, false, false)]
        public void When_OnTouchClicked_Called(float touchX, float touchY, bool graphicsLayerVisible, bool gameObjectVisible, bool expectedResult)
        {
            // Arrange
            bool result = false;
            GraphicsLayer graphicsLayer = new GraphicsLayer()
            {
                Visible = graphicsLayerVisible
            };
            SKTouchEventArgs sKTouchEventArgs = new SKTouchEventArgs(1, SKTouchAction.Released, new SKPoint(touchX, touchY), true);
            var obj = new GameObject
            {
                Position = new Vector2(20, 20),
                Width = 20,
                Height = 20,
                VisualSize = new Vector2(1, 1),
                Visible = gameObjectVisible
            };
            obj.ClickedEvent += (args, e) => result = true;

            // Act

            obj.OnTouchClicked(sKTouchEventArgs, graphicsLayer);

            // Assert
            Assert.That(result, Is.EqualTo(expectedResult));
        }

        [TestCase(10, 10, true, true, true)]
        [TestCase(5, 5, true, true, false)]
        [TestCase(25, 25, true, true, true)]
        [TestCase(30, 30, true, true, true)]
        [TestCase(31, 31, true, true, false)]
        [TestCase(10, 10, false, true, false)]
        [TestCase(5, 5, false, true, false)]
        [TestCase(25, 25, false, true, false)]
        [TestCase(30, 30, false, true, false)]
        [TestCase(31, 31, false, true, false)]
        [TestCase(10, 10, true, false, false)]
        [TestCase(5, 5, true, false, false)]
        [TestCase(25, 25, true, false, false)]
        [TestCase(30, 30, true, false, false)]
        [TestCase(31, 31, true, false, false)]
        public void When_OnTouchReleased_Called(float touchX, float touchY, bool graphicsLayerVisible, bool gameObjectVisible, bool expectedResult)
        {
            // Arrange
            bool result = false;
            GraphicsLayer graphicsLayer = new GraphicsLayer()
            {
                Visible = graphicsLayerVisible
            };
            SKTouchEventArgs sKTouchEventArgs = new SKTouchEventArgs(1, SKTouchAction.Released, new SKPoint(touchX, touchY), true);
            var obj = new GameObject
            {
                Position = new Vector2(20, 20),
                Width = 20,
                Height = 20,
                VisualSize = new Vector2(1, 1),
                Visible = gameObjectVisible
            };
            obj.ReleasedEvent += (args, e) => result = true;

            // Act

            obj.OnTouchReleased(sKTouchEventArgs, graphicsLayer);

            // Assert
            Assert.That(expectedResult, Is.EqualTo(result));
        }

        [Test]
        public void Resize_ShouldScalePositionAndScaleCorrectly()
        {
            // Arrange
            var obj = new GameObject
            {
                Position = new Vector2(100, 100),
                VisualSize = new Vector2(1, 1)
            };
            var oldRes = new Vector2(800, 600);
            var newRes = new Vector2(1600, 1200);

            // Act
            obj.Resize(oldRes, newRes);

            // Assert
            Assert.That(obj.Position, Is.EqualTo(new Vector2(200, 200)));
            Assert.That(obj.VisualSize, Is.EqualTo(new Vector2(1, 1)));
        }

        [Test]
        public void Draw_ShouldReturnImmediatelyIfNoAnimations()
        {
            // Arrange
            var obj = new GameObject { Visible = true };
            obj.Draw(null, null);
        }
    }
}