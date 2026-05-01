using SevenWonders.Game.Engine;
using SkiaSharp;
using SkiaSharp.Views.Maui;
using System.Numerics;

namespace SevenWonders.GameEngine_UnitTests
{
    [TestFixture]
    public class ButtonObjectTests
    {
        private ButtonObject m_button;

        [SetUp]
        public void Setup()
        {
            m_button = new ButtonObject
            {
                Id = 1,
                Name = "TestButton",
                Position = new Vector2(100, 100),
                Width = 50,
                Height = 30,
                Scale = new Vector2(1, 1),
                Visible = true
            };
        }

        [Test]
        public void When_DefaultConstructor_Called()
        {
            var button = new ButtonObject();
            Assert.That(button, Is.Not.Null);
        }

        [Test]
        public void When_CopyConstructor_Called()
        {
            var copy = new ButtonObject(m_button);

            Assert.Multiple(() =>
            {
                Assert.That(copy.Id, Is.EqualTo(m_button.Id));
                Assert.That(copy.Name, Is.EqualTo(m_button.Name));
                Assert.That(copy.Position, Is.EqualTo(m_button.Position));
                Assert.That(copy.Width, Is.EqualTo(m_button.Width));
                Assert.That(copy.Height, Is.EqualTo(m_button.Height));
            });
        }

        [Test]
        public void When_EqualsCalled_And_SameValues_ShouldReturnTrue()
        {
            var other = new ButtonObject
            {
                Id = 1,
                Name = "TestButton",
                Position = new Vector2(100, 100),
                Width = 50,
                Height = 30,
                Scale = new Vector2(1, 1),
                Visible = true
            };

            Assert.That(m_button.Equals(other), Is.True);
        }

        [Test]
        public void When_Equals_Called_With_Null_ShouldReturnFalse()
        {
            Assert.That(m_button.Equals((ButtonObject?)null), Is.False);
        }

        [Test]
        public void When_Clone_Called_ShouldReturnButtonObject()
        {
            var clone = m_button.Clone();

            Assert.That(clone, Is.TypeOf<ButtonObject>());
            Assert.That(clone, Is.EqualTo(m_button));
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
        public void When_OnTouchMoved_Called(float touchX, float touchY, bool graphicsLayerVisible, bool buttonObjectVisible, bool expectedResult)
        {
            // Arrange
            bool result = false;
            GraphicsLayer graphicsLayer = new GraphicsLayer()
            {
                Visible = graphicsLayerVisible
            };
            SKTouchEventArgs sKTouchEventArgs = new SKTouchEventArgs(1, SKTouchAction.Moved, new SKPoint(touchX, touchY), true);
            var obj = new ButtonObject
            {
                Position = new Vector2(20, 20),
                Width = 20,
                Height = 20,
                Visible = buttonObjectVisible
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
        public void When_OnTouchClicked_Called(float touchX, float touchY, bool graphicsLayerVisible, bool buttonObjectVisible, bool expectedResult)
        {
            // Arrange
            bool result = false;
            GraphicsLayer graphicsLayer = new GraphicsLayer()
            {
                Visible = graphicsLayerVisible
            };
            SKTouchEventArgs sKTouchEventArgs = new SKTouchEventArgs(1, SKTouchAction.Released, new SKPoint(touchX, touchY), true);
            var obj = new ButtonObject
            {
                Position = new Vector2(20, 20),
                Width = 20,
                Height = 20,
                Visible = buttonObjectVisible
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
        public void When_OnTouchReleased_Called(float touchX, float touchY, bool graphicsLayerVisible, bool buttonObjectVisible, bool expectedResult)
        {
            // Arrange
            bool result = false;
            GraphicsLayer graphicsLayer = new GraphicsLayer()
            {
                Visible = graphicsLayerVisible
            };
            SKTouchEventArgs sKTouchEventArgs = new SKTouchEventArgs(1, SKTouchAction.Released, new SKPoint(touchX, touchY), true);
            var obj = new ButtonObject
            {
                Position = new Vector2(20, 20),
                Width = 20,
                Height = 20,
                Visible = buttonObjectVisible
            };
            obj.ReleasedEvent += (args, e) => result = true;

            // Act

            obj.OnTouchReleased(sKTouchEventArgs, graphicsLayer);

            // Assert
            Assert.That(expectedResult, Is.EqualTo(result));
        }
    }
}
